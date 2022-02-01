using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Web.Models;
namespace PetStore.Web.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        ResponseDto ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest ApiRequest);
    }
}
