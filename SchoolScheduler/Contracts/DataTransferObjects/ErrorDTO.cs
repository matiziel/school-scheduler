using System;

namespace Contracts.DataTransferObjects {
    public class ErrorDTO {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
