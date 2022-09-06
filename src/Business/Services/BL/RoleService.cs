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
                    if (_unitOfWork.RolePermissionRepository.Any(x => x.RoleId == model.Id && x.PermissionId == permission.Id))
                    {
                        response.Status = "404";
                        response.Message = "{this permission} already exists for {this role}";
                        return response;

                    }
                    else
                    {
                        _unitOfWork.RolePermissionRepository.Add(new Repository.Entites.RolePermission()
                
                        {
                            RoleId = model.Id,
                            PermissionId = permission.Id
                        });
                        _unitOfWork.Save();
                    }

                    response.Status = "404";
                    response.Message = "Cannot assign permission to role";
                    return response;
                }
            
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


            #region RemovePermission
            //var rolePermission = _unitOfWork._db.RolePermissions.FirstOrDefault(x => x.RoleId == model.Id && x.PermissionId == model.Id);
            //if(rolePermission == null)
            //{
            //    response.Status = "404";
            //    response.Message = "Permission isnot assign in this role";
            //    return response;
            //}
            //_unitOfWork._db.RolePermissions.Remove(rolePermission);
            //_unitOfWork._db.SaveChanges();

            //response.Status = "00";
            //response.Message = "Permission removed sucessfully";
            //return response;
            #endregion
        }
    }
    public interface IRoleService
    {
        BaseResponseModel<string> CreateRole(RoleViewModel model);
        BaseResponseModel<string> ManageRole(EditRole model);
        BaseResponseModel<string> DeleteRole(Role model);
        //BaseResponseModel<string> AssignPermissionToRole(string roleId, string[] permissionId);
        BaseResponseModel<string> RemovePermissionFromRole(string roleId, string permissionId);
    }
}