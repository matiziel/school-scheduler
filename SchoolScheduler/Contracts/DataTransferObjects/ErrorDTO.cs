using System;

namespace Contracts.DataTransferObjects {
    public class ErrorDTO {
        public string Message { get; set; }
        public ErrorDTO() { }
        public ErrorDTO(string message) => Message = message;
    }
}
