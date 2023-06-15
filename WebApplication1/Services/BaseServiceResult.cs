namespace CampaingControlCenterAPI.Services
{
    public abstract class BaseServiceResult
    {

        public bool IsSuccess
        {
            get
            {
                return ErrorCode == ErrorCodeEnum.Success;
            }
        }

        public string? ErrorMessage { get; set; }

        public ErrorCodeEnum ErrorCode { get; set; }
    }

    public enum ErrorCodeEnum
    {
        Success = 0,
        NotFound = 404,
        DatabaseError = 4001,
        InternalServerError = 500,
        BadRequest = 400,
        Unauthorized = 4002
    }
    public class ServiceResult<T> : BaseServiceResult
    {
        public T Result { get; set; }
    }

    public class ServiceResult : BaseServiceResult
    {
    }
}
