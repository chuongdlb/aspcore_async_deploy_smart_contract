﻿using System;
using System.ComponentModel.DataAnnotations;

namespace aspcore_async_deploy_smart_contract.Dal.Entities
{
    public enum DeployStatus
    {
        Pending,
        ErrorInDeploy,
        Retrying,
        Querrying,
        ErrorInQuerrying,
        DoneQuerrying
    }

    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }

        public string OrganizationId { get; set; }
        public string ContractAddress { get; set; }

        public DateTime DeployStart { get; set; }
        public DateTime DeployDone { get; set; }
        public DateTime QuerryDone { get; set; }

        public DeployStatus Status { get; set; }
        public string Messasge { get; set; }

        public string TransactionId { get; set; }
        public string Hash { get; set; }
    }
}
