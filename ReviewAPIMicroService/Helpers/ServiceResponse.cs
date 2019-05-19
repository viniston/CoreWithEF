using System;

namespace ReviewAPIMicroService.Helpers
{
    public class ServiceResponse<T>
    {
        public ServiceResponse() : this(default(T)) { }

        public ServiceResponse(T data)
        {
            State = Object.Equals(data, default(T)) ? ServiceResponseState.Failed : ServiceResponseState.Success;
            Data = data;
        }

        public int ResultCode = 0;

        public ServiceResponseState State { get; set; }

        public T Data { get; }

        public string Message { get; set; } = String.Empty;
    }

    public enum ServiceResponseState
    {
        NotAuth = -1,
        Failed = 0,
        Success = 1
    }

}
