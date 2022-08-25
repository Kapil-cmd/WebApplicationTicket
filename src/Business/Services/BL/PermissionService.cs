using Common.ViewModels.BaseModel;
using Common.ViewModels.Permission;
using Repository.Repos.Work;

namespace Services.BL
{
    public class PermissionService : IPermissionService
    {
        public readonly IUnitOfWork _unitOfWork;
        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<string> AddPermission(AddPermission model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                if (_unitOfWork.Permission.Any(x => x.PermissionId == model.PermissionId))
                {
                    response.Status = "97";
                    response.Message = "Permission with same name already exists";
                    return response;
                }

                _unitOfWork._db.Permissions.Add(new Repository.Entites.Permission()
                {
                    Name = model.Name,
                    ActionName = model.ActionName,
                    ControllerName = model.ControllerName,
                });
                _unitOfWork._db.SaveChanges();
                response.Status = "00";
                response.Message = "Permission created sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "100";
                response.Message = "Error occurred :" + ex.Message;
                return response;
            }

        }
        public BaseResponseModel<string> EditPermission(EditPermission model)
        {
           var response = new BaseResponseModel<string>();
            {
                try
                {
                    var permission = _unitOfWork._db.Permissions.FirstOrDefault(x => x.PermissionId == model.PermissionId);
                    if(permission ==null)
                    {
                        response.Status = "97";
                        response.Message = "Permission not found";
                        return response;
                    }
                    permission.PermissionId = model.PermissionId;
                    permission.Name = model.Name;
                    permission.ActionName = model.ActionName;
                    permission.ControllerName = model.ControllerName;

                    _unitOfWork._db.Update(model);
                    _unitOfWork._db.SaveChanges();

                    response.Status = "00";
                    response.Status = "Permission edited sucessfully";
                    return response;
                }
                catch(Exception ex)
                {
                    response.Status = "500";
                    response.Message = "Error occurred :" + ex.Message;
                    return response;
                }
            }
        }

        public BaseResponseModel<string> DeletePermission(PermissionViewModel model)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var permission = _unitOfWork._db.Permissions.FirstOrDefault(x => x.PermissionId == model.PermissionId);
                if (permission == null) ;
                {
                    response.Status = "97";
                    response.Message = "Permission with id{permissionId} not found";
                    return response ;
                }
                permission.PermissionId=model.PermissionId;
                permission.Name = model.Name;

                _unitOfWork._db.Remove(model);
                _unitOfWork.Save();

                response.Status = "00";
                response.Message = "Permission deleted sucessfully";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occurred:" + ex.Message;
                return response;
            }
        }

    }
    public interface IPermissionService
    {
        BaseResponseModel<string> AddPermission(AddPermission model);
        BaseResponseModel<string> EditPermission(EditPermission model);
        BaseResponseModel<string> DeletePermission(PermissionViewModel model);
    }
}
