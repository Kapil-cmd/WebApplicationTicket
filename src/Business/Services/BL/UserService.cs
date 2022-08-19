using Common.ViewModels.BaseModel;
using Common.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Repos.Work;
using System.Security.Claims;
using System.Web.Helpers;

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
                    response.Status = "97";
                    response.Message = "User not found";
                    return response;
                }
                #region
                model.Password = Crypto.Hash(model.Password);
                #endregion


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
                Register.Password = Crypto.Hash(Register.Password);
                #endregion
                Register.IsEmailVerified = false;

                _unitOfWork._db.Users.Add(new Repository.Entites.User()
                {
                    Address = Register.Address,
                    Age = Register.Age,
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

                _unitOfWork._db.Users.Update(user);
                _unitOfWork._db.SaveChanges();
                #endregion

                #region AssignRole
                if (_unitOfWork.UserRoleRepository.Any(x => x.UserId == EditUser.Id && x.RoleId == EditUser.Role))
                {
                    response.Status = "444";
                    response.Message = "This role is already exists for {this user}";
                    return response;
                }
                _unitOfWork.UserRoleRepository.Add(new Repository.Entites.UserRole()
                {
                    RoleId = EditUser.Role,
                    UserId = EditUser.Id,
                });
                _unitOfWork.Save();


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
        public BaseResponseModel<string> DeleteUser(UserViewModel DeleteUser)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var user = _unitOfWork._db.Users.Include("Roles").Include("Roles.aRole").FirstOrDefault(x => x.Id == DeleteUser.Id);
                if (user == null)
                {
                    response.Status = "100";
                    response.Message = "User not found";
                    return response;
                }
                user.Id = DeleteUser.Id;
                user.FirstName = DeleteUser.FirstName;
                user.LastName = DeleteUser.LastName;
                user.UserName = DeleteUser.UserName;
                user.Address = DeleteUser.Address;
                user.Age = DeleteUser.Age;
                user.Email = DeleteUser.Email;
                user.PhoneNumber = DeleteUser.PhoneNumber;

                _unitOfWork._db.Users.Remove(user);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "User removed sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " +ex.Message;
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
                user.Password = model.NewPassword = Crypto.Hash(model.NewPassword);

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



        //public BaseResponseModel<string> AssignUserToRole(string userId, string roleId)
        //{
        //    BaseResponseModel<string> response = new BaseResponseModel<string>();
        //    try
        //    {
        //        if (_unitOfWork.UserRoleRepository.Any(x => x.UserId == userId && x.RoleId == roleId))
        //        {
        //            response.Status = "444";
        //            response.Message = "{this role} already exists for {this user}";
        //            return response;
        //        }
        //        _unitOfWork.UserRoleRepository.Add(new Repository.Entites.UserRole()
        //        {
        //            RoleId = roleId,
        //            UserId = userId
        //        });
        //        _unitOfWork.Save();

        //        response.Status = "00";
        //        response.Message = "Successfully assigned {this user} to {this role}";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = "500";
        //        response.Message = "Error Occured : " + ex.Message;
        //        return response;
        //    }

        //}
        //public BaseResponseModel<string> RemoveUserFromRole(string userId, string roleId)
        //{

        //    BaseResponseModel<string> response = new BaseResponseModel<string>();
        //    try
        //    {
        //        var userRole = _unitOfWork.UserRoleRepository.GetFirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
        //        if (userRole == null)
        //        {
        //            response.Status = "404";
        //            response.Message = "{this role} has not been yet assigned to {this user}";
        //            return response;
        //        }

        //        _unitOfWork.UserRoleRepository.Remove(userRole);
        //        _unitOfWork.Save();

        //        response.Status = "00";
        //        response.Message = "Successfully removed {this user} from {this role}";

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = "500";
        //        response.Message = "Error Occured : " + ex.Message;
        //        return response;
        //    }

        //}
    }

    public interface IUserService
    {
        Task<BaseResponseModel<UserTokenModel>> LoginAsync(UserLogin Login);
        BaseResponseModel<string> Register(UserRegister Register);
        BaseResponseModel<string> EditUser(EditUserViewModel Edituser);
        BaseResponseModel<string> DeleteUser(UserViewModel DeleteUser);
        BaseResponseModel<string> ChangePassword(ChangePassword model);
        //BaseResponseModel<string> ForgetPassword (ForgetPassword forgetPassword)
        
    }
}

