﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace huhu.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class huhuEntities : DbContext
    {
        public huhuEntities()
            : base("name=huhuEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<admin_all> admin_all { get; set; }
        public virtual DbSet<admin_manager> admin_manager { get; set; }
        public virtual DbSet<advert_all> advert_all { get; set; }
        public virtual DbSet<area_code_all> area_code_all { get; set; }
        public virtual DbSet<article_comment> article_comment { get; set; }
        public virtual DbSet<article_draft> article_draft { get; set; }
        public virtual DbSet<article_reply> article_reply { get; set; }
        public virtual DbSet<article_view> article_view { get; set; }
        public virtual DbSet<feedback_all> feedback_all { get; set; }
        public virtual DbSet<notify_all> notify_all { get; set; }
        public virtual DbSet<report_all> report_all { get; set; }
        public virtual DbSet<report_option> report_option { get; set; }
        public virtual DbSet<topic_all> topic_all { get; set; }
        public virtual DbSet<topic_circle> topic_circle { get; set; }
        public virtual DbSet<user_collect> user_collect { get; set; }
        public virtual DbSet<user_collection> user_collection { get; set; }
        public virtual DbSet<user_digg> user_digg { get; set; }
        public virtual DbSet<user_follow> user_follow { get; set; }
        public virtual DbSet<user_locate> user_locate { get; set; }
        public virtual DbSet<article_all> article_all { get; set; }
        public virtual DbSet<article_tag> article_tag { get; set; }
        public virtual DbSet<user_all> user_all { get; set; }
    }
}