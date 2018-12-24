﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using aspcore_async_deploy_smart_contract.Dal;

namespace aspcore_async_deploy_smart_contract.Dal.Migrations
{
    [DbContext(typeof(BECDbContext))]
    [Migration("20181219092955_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("aspcore_async_deploy_smart_contract.Dal.Entities.Certificate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContractAddress");

                    b.Property<DateTime>("DeployDone");

                    b.Property<DateTime>("DeployStart");

                    b.Property<string>("Hash");

                    b.Property<string>("Messasge");

                    b.Property<string>("OrganizationId");

                    b.Property<DateTime>("QuerryDone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("TaskId");

                    b.Property<string>("TransactionId");

                    b.HasKey("Id");

                    b.ToTable("Certificates");
                });
#pragma warning restore 612, 618
        }
    }
}
