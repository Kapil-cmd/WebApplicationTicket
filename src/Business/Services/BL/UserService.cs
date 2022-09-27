using Common.ViewModels.BaseModel;
using Common.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.Repos.Work;
using Services.Hash;
using System.Security.Claims;

namespace Services.BL
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponseModel<UserTokenModel>> LoginAsync(UserLogin model)
        {
            var response = new BaseResponseModel<UserTokenModel>();

            try
            {
                var user = _unitOfWork._db.Users.Include("Roles").Include("Roles.aRole").FirstOrDefault(x => x.UserName == model.UserName);
                if (user == null)
                {
                    response.Status = "404";
                    response.Message = "User not found";
                    return response;
                }
                #region
                model.Password = HashPassword.Hash(model.Password);
                #endregion
                if(user.Status == false)
                {
                    response.Status = "97";
                    response.Message = "You are suspended";
                    return response;
                }

                if (user.Password != model.Password )
                {
                    response.Status = "97";
                    response.Message = "Invalid Password Provided";
                    return response;
                }
                if(user.UserName != model.UserName)
                {
                    response.Status = "97";
                    response.Message = "Invalid UserName provided";
                    return response;
                }

                UserTokenModel data = new UserTokenModel()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Id = user.Id,
                    Roles = user.Roles.Select(x => x.aRole.Name).ToList()
                };


                var authProperties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                    IsPersistent = true,
                };
               

                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, data.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Actor, data.Email));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, data.FirstName + " " + data.LastName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, data.Id));
                foreach (var item in data.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, item));
                }
                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), authProperties
                    );
                response.Status = "00";
                response.Message = "Successfully logged in.";
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occured: " + ex.Message;
                return response;
            }
            return response;
        }
        public BaseResponseModel<string> Register(UserRegister Register)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                if (_unitOfWork.UserRepository.Any(x => x.UserName == Register.UserName))
                {
                    response.Status = "98";
                    response.Message = "User with this username already exists";
                    return response;
                }
               

                #region Generate Activation Code
                Register.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                Register.Password = HashPassword.Hash(Register.Password);
                #endregion
                Register.IsEmailVerified = false;
                Register.Status = true;

                _unitOfWork._db.Users.Add(new Repository.Entites.User()
                {
                    Address = Register.Address,
                    Age = DateTime.Now.Year - Register.DateOfBirth.Year,
                    Email = Register.Email,
                    FirstName = Register.FirstName,
                    LastName = Register.LastName,
                    Password = Register.Password,
                    PhoneNumber = Register.PhoneNumber,
                    UserName = Register.UserName,
                    IsEmailVerified = Register.IsEmailVerified,
                    ActivationCode = Register.ActivationCode,
                });   
                _unitOfWork._db.SaveChanges();
                
                

                response.Status = "00";
                response.Message = "User successfully registered";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<string> EditUser(EditUserViewModel EditUser)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == EditUser.Id);
                if (user == null)
                {
                    response.Status = "97";
                    response.Message = "User not found";
                    return response;
                }

                #region UserUpdate
                user.Address = EditUser.Address;
                user.Email = EditUser.Email;
                user.FirstName = EditUser.FirstName;
                user.LastName = EditUser.LastName;
                user.PhoneNumber = EditUser.PhoneNumber;
                user.UserName = EditUser.UserName;

                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();
                #endregion

                #region AssignRole
                foreach(var role in EditUser.Roles)
                {
                    if(role.IsSelected == true)
                    { 
                        if(_unitOfWork._db.UserRoles.Any(x => x.UserName == EditUser.UserName && x.RoleName == role.Name))
                        {
                            response.Status = "97";
                            response.Message = "Role already exits";
                        }
                        else
                        { 
                            _unitOfWork._db.UserRoles.Add(new Repository.Entites.UserRole()
                            {
                                UserName = EditUser.UserName,
                                RoleName = role.Name
                            });
                            _unitOfWork.Save();
                        }
                    }
                    else
                    {
                        if (_unitOfWork._db.UserRoles.Any(x => x.UserName == EditUser.UserName && x.RoleName == role.Name))
                        {
                            var userRoles = _unitOfWork._db.UserRoles.FirstOrDefault(x => x.UserName == EditUser.UserName && x.RoleName == role.Name);
                            _unitOfWork._db.UserRoles.Remove(userRoles);
                            _unitOfWork._db.SaveChanges();
                        }
                    }
                }
                #endregion
               
                response.Status = "00";
                response.Message = "User Edited sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
       
      public BaseResponseModel<string> ChangePassword(ChangePassword model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var Id = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == Id);

                model.Id = user.Id;

                if (_unitOfWork.UserRepository.Any(x => x.Password == model.Password))
                {
                    response.Status = "98";
                    response.Message = "Password matched";
                    return response;
                   
                }
                user.Password = model.NewPassword = HashPassword.Hash(model.NewPassword);

                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Password Changed Successfully";
                return response;
                
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message ="Error Occurred: " + ex.Message;
                return response;
            }
        }
      
        public BaseResponseModel<string> ResetPassword(ResetPassword model)
        {
            BaseResponseModel<string> response= new BaseResponseModel<string>();
            try
            {
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.OTP == model.OTP);
                if (user == null)
                {
                    response.Status = "97";
                    response.Message = "User not found";
                    return response;
                }
                if (_unitOfWork._db.Users.Any(x => x.OTP == model.OTP))
                {
                    user.Password = model.NewPassword = HashPassword.Hash(model.NewPassword);
                    _unitOfWork._db.Users.Update(user);
                    _unitOfWork._db.SaveChanges();
                }
                response.Status = "00";
                response.Message = "Password reset sucessfully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<string> UserProfile(UserProfile model)
        {
            var response= new BaseResponseModel<string>();
            try
            {
                var id = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == id);
                if(user == null)
                {
                    response.Status = "404";
                    response.Message = "User Not Found";
                    return response;
                }
                user.ProfilePicture = model.ImageName;

                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Profile picture added sucessfully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred :" + ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> UserStatus(string Id)
        {
            var response = new BaseResponseModel<string>(); try
            {
                var user = _unitOfWork._db.Users.FirstOrDefault(x => x.Id == Id);
                if (user == null)
                {
                    response.Status = "404";
                    response.Message = "User not found";
                    return response;
                }
                if (_unitOfWork._db.Users.Any(x => x.UserName == user.UserName && x.Status == false))
                {
                    user.Status = true;
                    _unitOfWork._db.Update(user);
                    _unitOfWork._db.SaveChanges();
                }
                else
                {
                    user.Status = false;
                    _unitOfWork._db.Users.Update(user);
                    _unitOfWork._db.SaveChanges();
                }
                    response.Status = "00";
                    response.Message = "User suspended";
                    return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "ExceptionOccured :" + ex.Message;
                return response;
            }
        }
    }

    public interface IUserService
    {
        Task<BaseResponseModel<UserTokenModel>> LoginAsync(UserLogin Login);
        BaseResponseModel<string> Register(UserRegister Register);
        BaseResponseModel<string> EditUser(EditUserViewModel Edituser);
        BaseResponseModel<string> ChangePassword(ChangePassword model);
        BaseResponseModel<string> ResetPassword(ResetPassword model);
        BaseResponseModel<string> UserProfile(UserProfile model);
        BaseResponseModel<String> UserStatus(string Id);
    }
}

