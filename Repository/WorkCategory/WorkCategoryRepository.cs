﻿
using Microsoft.Extensions.Configuration;
using Repository.Data;
using Shared.Library;
using Shared.WorkCategory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Repository.WorkCategory
{
    public class WorkCategoryRepository : IWorkCategoryRepository
    {
        private readonly IDataAccess _dataAccess;
        public WorkCategoryRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public MainWorkCategory List()
        {
            MainWorkCategory workCat = new MainWorkCategory
            {
                mainWorkCategory = new List<WorkCategoryModel>()
            };
            var sql = "Exec proc_WorkCategory @flag='GetAll'";
            var dr = _dataAccess.ExecuteDataset(sql);
            DataTable dt1 = dr.Tables[0];
            DataTable dt2 = dr.Tables[1];
            foreach (DataRow ds1 in dt1.Rows)
            {
                WorkCategoryModel work = new WorkCategoryModel
                {
                    Id = ds1["ID"].ToString(),
                    CreatedBy = ds1["CreatedBy"].ToString(),
                    CreatedDate = ds1["CreatedDate"].ToString(),
                    Description = ds1["Description"].ToString(),
                    ImageUrl = ds1["ImageUrl"].ToString(),
                    WorkType = ds1["WorkType"].ToString(),
                    CategoryName = ds1["CategoryName"].ToString(),
                    CategoryCode = ds1["CategoryCode"].ToString()
                };
                List<WorkSubCategoryModel> cl = new List<WorkSubCategoryModel>();
                foreach (DataRow ds2 in dt2.Rows)
                {
                    var cc = new WorkSubCategoryModel();
                    if (ds1["CategoryCode"].ToString() == ds2["CategoryCode"].ToString())
                    {
                        cc.CreatedBy = ds2["CreatedBy"].ToString();
                        cc.CreatedDate = ds2["CreatedDate"].ToString();
                        cc.Description = ds2["Description"].ToString();
                        cc.ImageUrl = ds2["ImageUrl"].ToString();
                        cc.CategoryCode = ds2["CategoryCode"].ToString();
                        cc.SubCategoryName = ds2["SubCategoryName"].ToString();
                        cl.Add(cc);
                    }
                }
                work.WorkSubCategory = cl;
                workCat.mainWorkCategory.Add(work);
            }
            return workCat;
        }            
        public WorkCategoryModel GetById(int? id)
        {
            var sql = "Exec proc_WorkCategory @flag='GetById'";
            sql += ", @id=" + id;
            var dt = _dataAccess.ExecuteDataRow(sql);
            WorkCategoryModel details = new WorkCategoryModel();
            if (dt.Table.Rows.Count == 1)
            {
                details.CreatedBy = dt["CreatedBy"].ToString();
                details.CreatedDate = dt["CreatedDate"].ToString();
                details.Description = dt["Description"].ToString();
                details.ImageUrl = dt["ImageUrl"].ToString();
                details.WorkType = dt["WorkType"].ToString();
                details.CategoryName = dt["CategoryName"].ToString();
                details.CategoryCode = dt["CategoryCode"].ToString();
            }
            else
            {

            }
            return details;
        }
        public DataRow Create(WorkCategoryModel model)
        {
            var sql = "Exec proc_WorkCategory @flag='Create'";
            sql += ", @CategoryName=" + _dataAccess.FilterString(model.CategoryName.ToString());
            sql += ", @CategoryCode=" + _dataAccess.FilterString(model.CategoryCode.ToString());
            var ct = _dataAccess.ExecuteDataRow(sql);
            return ct;
        }
        public DbResponse Update(WorkCategoryModel model)
        {
            var sql = "Exec proc_WorkCategory @flag='Update'";
            sql += ", @id=" + model.Id;
            sql += ", @CategoryName=" + _dataAccess.FilterString(model.CategoryName.ToString());
            sql += ", @CategoryCode=" + _dataAccess.FilterString(model.CategoryCode.ToString());
            var dt = _dataAccess.ExecuteDataRow(sql);
            if (dt != null)
            {
                var response = new DbResponse
                {
                    Code = dt["Code"].ToString(),
                    Message = dt["Message"].ToString(),
                    Extra = dt["Extra"].ToString()
                };
                return response;
            }
            else
            {
                DbResponse result = new DbResponse();
                return result;
            }
        }
        public DbResponse Delete(WorkCategoryModel model)
        {
            var sql = "Exec proc_WorkCategory @flag='Delete'";
            sql += ", @id=" + model.Id;
            var dt = _dataAccess.ExecuteDataRow(sql);
            if (dt != null)
            {
                var response = new DbResponse
                {
                    Code = dt["Code"].ToString(),
                    Message = dt["Message"].ToString(),
                    Extra = dt["Extra"].ToString()

                };
                return response;
            }
            else
            {
                DbResponse result = new DbResponse();
                return result;
            }
        }
    }
}
