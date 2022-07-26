using Common.ViewModels.BaseModel;
using Common.ViewModels.Categories;
using Repository;
using Repository.Repos.Work;
using System.Security.Claims;

namespace Services.BL
{
    public class CategoryServices:ICategoryservice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketingContext _db;
        private readonly IUserService _userService;
        public CategoryServices(IUnitOfWork unitOfWork,TicketingContext db, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userService = userService;
        }
        public  BaseResponseModel<int> AddCategory(AddCategoryViewModel model)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

                model.CreatedBy = nameClaim;
                model.CreatedDateTime = DateTime.Now;
                if (_unitOfWork._db.Category.Any(x => x.CategoryName == x.CategoryName))
                {
                    response.Status = "97";
                    response.Message = "Cannot create this category as the category with this name already exists";
                    return response;
                }
                _unitOfWork._db.Category.Add(new Repository.Entites.Category()
                {
                    CategoryName = model.CategoryName,
                    CreatedBy = model.CreatedBy,
                    CreatedDateTime = model.CreatedDateTime,
                });
              
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Category added sucessfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured" + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> EditCategory(EditCategoryViewModel EditCategory)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var category = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == EditCategory.CId);
                if(category == null)
                {
                    response.Status = "100";
                    response.Message = "Category not Found";
                    return response;
                }
                category.CategoryName = EditCategory.CategoryName;
                category.ModifiedDateTime = EditCategory.ModifiedDateTime;
                category.ModifiedBy = EditCategory.ModifiedBy;

                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

                EditCategory.ModifiedBy = nameClaim;
                EditCategory.ModifiedDateTime = DateTime.Now;

                _unitOfWork._db.Category.Update(category);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Category Updated Sucessfully";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> DeleteCategory(CategoryViewModel DeleteCategory)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var category = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == DeleteCategory.CId);
                if(category == null)
                {
                    response.Status = "100";
                    response.Message = "Category not found";
                    return response;
                }
                category.CId = DeleteCategory.CId;
                category.CreatedBy = DeleteCategory.CreatedBy;
                category.CreatedDateTime = DeleteCategory.CreatedDateTime;
                category.ModifiedDateTime= DeleteCategory.ModifiedDateTime;
                category.ModifiedBy = DeleteCategory.ModifiedBy;
                category.CategoryName = DeleteCategory.CategoryName;
            
                _unitOfWork._db.Category.Remove(category);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Category deleted sucessfully";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message ="Error occured: "+ ex.Message;
                return response;
            }
        }
        public BaseResponseModel<int> CategoryDetails(int CategoryId)
        {
            var response = new BaseResponseModel<int>();
            try
            {
                var category = _unitOfWork._db.Category.Find(CategoryId);
                if (category == null)
                {
                    response.Status = "100";
                    response.Message = "Category not found";
                    return response;
                }
               

                response.Status = "00";
                response.Message = "Category Details";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }

    }
    public interface ICategoryservice
    {
        BaseResponseModel<int> AddCategory(AddCategoryViewModel model);
        BaseResponseModel<int> EditCategory(EditCategoryViewModel EditCategory);
        BaseResponseModel<int> DeleteCategory(CategoryViewModel DeleteCategory);
        BaseResponseModel<int> CategoryDetails(int CategoryId);

    }
}
