﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Chinook.Domain.DbInfo;
using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using RepoDb;

namespace Chinook.DataRepoDb.Repositories
{
    public class MediaTypeRepository : IMediaTypeRepository
    {
        private readonly DbInfo _dbInfo;

        public MediaTypeRepository(DbInfo dbInfo)
        {
            _dbInfo = dbInfo;
            RepoDb.SqlServerBootstrap.Initialize();
        }

        private IDbConnection Connection => new SqlConnection(_dbInfo.ConnectionStrings);

        public void Dispose()
        {
            
        }

        private bool MediaTypeExists(int id) =>
            Connection.Exists("select count(1) from MediaType where MediaTypeId = @id", new {id});

        public List<MediaType> GetAll()
        {
            using IDbConnection cn = Connection;
            cn.Open();
            var mediaTypes = cn.QueryAll<MediaType>();
            return mediaTypes.ToList();
        }

        public MediaType GetById(int id)
        {
            using var cn = Connection;
            cn.Open();
            return cn.Query<MediaType>(id).FirstOrDefault();
        }

        public MediaType Add(MediaType newMediaType)
        {
            using var cn = Connection;
            cn.Open();
            cn.Insert(newMediaType);
            return newMediaType;
        }

        public bool Update(MediaType mediaType)
        {
            if (!MediaTypeExists(mediaType.MediaTypeId))
                return false;

            try
            {
                using var cn = Connection;
                cn.Open();
                return (cn.Update(mediaType) > 0);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var cn = Connection;
                cn.Open();
                return cn.Delete(new MediaType {MediaTypeId = id}) > 0;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}