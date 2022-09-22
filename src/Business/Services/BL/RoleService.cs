using Common.ViewModels.BaseModel;
using Common.ViewModels.Role;
using Repository.Entites;
using Repository.Repos.Work;

namespace Services.BL
{
    public class RoleService : IRoleService
    {
        public readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<string> CreateRole(RoleViewModel model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                if(_unitOfWork.Role.Any(x => x.Name == model.Name))
                {
                    response.Status = "98";
                    response.Message = "Role with this name already exists";
                    return response;
                }
                _unitOfWork._db.Roles.Add(new Repository.Entites.Role()
                {
                    Name = model.Name,
                });
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Role added sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> DeleteRole(Role model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var role = _unitOfWork._db.Roles.FirstOrDefault(x => x.Id == model.Id);
                if(role == null)
                {
                    response.Status = "404";
                    response.Message = "Role not found";
                    return response;
                }
                role = model;
                _unitOfWork._db.Remove(model);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Role deleted sucessully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occureed :" + ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> ManageRole(EditRole model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var role = _unitOfWork._db.Roles.FirstOrDefault(x => x.Id == model.Id);
                if (role == null)
                {
                    response.Status = "97";
                    response.Message = "Role not found";
                    return response;
                }

                #region RoleUpdate
                role.Id = model.Id;
                role.Name = model.Name;

                _unitOfWork._db.Update(role);
                _unitOfWork._db.SaveChanges();
                #endregion

                #region AssignPermission
                foreach (var permission in model.ListPermission)
                {
                    if (permission.IsPermitted == true)
                    {
                        //if (_unitOfWork.RolePermissionRepository.Any(x => x.RoleName == model.Name && x.PermissionId == permission.Id))
                        //{
                        //    var rolePermission = _unitOfWork._db.RolePermissions.FirstOrDefault(x => x.RoleName == model.Name && x.PermissionId == permission.Id);
                        //    _unitOfWork._db.RolePermissions.Remove(rolePermission);
                        //    _unitOfWork._db.SaveChanges();
                        //}
                        //else
                        {
                            _unitOfWork.RolePermissionRepository.Add(new Repository.Entites.RolePermission()
                            {
                                RoleName = model.Name,
                                PermissionId = permission.Id
                            });
                            _unitOfWork.Save();
                        }
                    }
                }

                #endregion

                response.Status = "00";
                response.Message = "Permission edited sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred:" + ex.Message;
                return response;

            }
        }
       
    }
    public interface IRoleService
    {
        BaseResponseModel<string> CreateRole(RoleViewModel model);
        BaseResponseModel<string> ManageRole(EditRole model);
        BaseResponseModel<string> DeleteRole(Role model);
    }
}