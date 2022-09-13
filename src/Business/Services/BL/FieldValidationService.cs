using Common.ViewModels.BaseModel;
using Common.ViewModels.ValidationModel;
using Repository.Repos.Work;

namespace Services.BL
{
    public class FieldValidationService : IFieldValidationService
    {
        private IUnitOfWork _unitOfWork;
        public FieldValidationService(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public BaseResponseModel<string> AddValdation(AddValidationField validationField)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                if(_unitOfWork._db.Field.Any(x => x.Name == validationField.Name))
                {
                    response.Status = "100";
                    response.Message = "Name matched";
                    return response;
                }
                else
                {
                    _unitOfWork._db.Field.Add(new Repository.Entities.FieldValidation()
                    {
                        Name = validationField.Name,
                        Length = validationField.Length,
                    });
                    _unitOfWork._db.SaveChanges();

                    response.Status = "00";
                    response.Message = "Validation Added sucessfully";
                    return response;
                }
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message ="Exception occurred: "+ ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> EditValidation(EditValidationField validationField)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var validate = _unitOfWork._db.Field.FirstOrDefault(x => x.Id == validationField.Id);
                if(validate == null)
                {
                    response.Status = "404";
                    response.Message = "Field not found";
                    return response;
                }
                else 
                {
                    validate.Name = validationField.Name;
                    validate.Length = validationField.Length;
                    _unitOfWork._db.Field.Update(validate);
                    _unitOfWork._db.SaveChanges();
                }
                response.Status = "00";
                response.Message = "Field edited sucessfully";
                return response;

            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occurred :"+ ex.Message;
                return response;
            }
        }
    }
    public interface IFieldValidationService
    {
         BaseResponseModel<string> AddValdation(AddValidationField validationField);
        BaseResponseModel<string> EditValidation(EditValidationField validationField);
    }
}
