using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels.BaseModel
{
    public class BaseResponseModel<T>
    {
        public string Status { get; set; }
        public string Message { get; set;  }
        public T Data { get; set; }

        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorProperty { get; set; }
    }
}
