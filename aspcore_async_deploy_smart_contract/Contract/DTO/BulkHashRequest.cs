﻿namespace aspcore_async_deploy_smart_contract.Contract.DTO
{
    public class BulkHashRequest
    {
        public string OrganizationId { get; set; }
        public string[] HashList { get; set; }
    }
}
