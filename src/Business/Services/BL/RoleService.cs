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

                #region Rolepdate
                role.Id = model.Id;
                role.Name = model.Name;

                _unitOfWork._db.Update(role);
                _unitOfWork._db.SaveChanges();
                #endregion

                #region AssignPermission
                if (_unitOfWork.RolePermissionRepository.Any(x => x.RoleId == model.Id && x.PermissionId == model.Permission))
                {
                    response.Status = "444";
                    response.Message = "{this permission} already exists for {this role}";
                    return response;
                }

                if (_unitOfWork.RolePermissionRepository.Any(x => x.RoleId == model.Id && x.PermissionId == model.Permission))
                {
                    response.Status = "444";
                    response.Message = "{this permission} already exists for {this role}";
                    return response;
                }
                _unitOfWork.RolePermissionRepository.Add(new Repository.Entites.RolePermission()
                {
                    RoleId = model.Id,
                    PermissionId = model.Permission,
                }); 
                _unitOfWork.Save();
                #endregion

                response.Status = "00";
                response.Message = "Role edited sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred:" + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<string> DeleteRole(Role model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var role = _unitOfWork._db.Roles.FirstOrDefault(x => x.Id == model.Id);
                if (role == null)
                {
                    response.Status = "404";
                    response.Message = "Role not found";
                    return response;
                }

                _unitOfWork._db.Roles.Remove(role);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Role removed sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred:" + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<string> AssignPermissionToRole(string roleId, string permissionId)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                if(_unitOfWork.RolePermissionRepository.Any(x => x.RoleId == roleId && x.PermissionId == permissionId))
                {
                    response.Status = "444";
                    response.Message = "{this permission} already exists for {this role}";
                    return response;
                }
                _unitOfWork.RolePermissionRepository.Add(new Repository.Entites.RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
                _unitOfWork.Save();
                response.Status = "00";
                response.Message = "Sucessfully assigned {this permission} to role";

                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured :" + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<string> RemovePermissionFromRole(string roleId, string permissionId)
        {
            BaseResponseModel<string> response = new BaseResponseModel<string>();
            try
            {
                var rolePermission = _unitOfWork.RolePermissionRepository.GetFirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId);
                if(rolePermission == null)
                {
                    response.Status = "404";
                    response.Message = "{this permission} has not been yet assigned to this{this user}";
                    return response;
                }
                _unitOfWork.RolePermissionRepository.Remove(rolePermission);
                _unitOfWork.Save();

                response.Status = "00";
                response.Message = "Sucessfully removed permission from role";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message ="Error occured :" + ex.Message;
                return response;
            }
        }
    }
    public interface IRoleService
    {
        BaseResponseModel<string> CreateRole(RoleViewModel model);
        BaseResponseModel<string> ManageRole(EditRole model);
        BaseResponseModel<string> DeleteRole(Role model);
        BaseResponseModel<string> AssignPermissionToRole(string roleId, string permissionId);
        BaseResponseModel<string> RemovePermissionFromRole(string roleId, string permissionId);
    }
}
