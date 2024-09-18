

using NewDigitalPlatform.DataAssets.Imp;
using NewDigitalPlatform.Entities;
using System.Data;
using System.Data.SQLite;

namespace DigitaPlatForm.DataAccess
{
    public class LocalDataAccess : ILocalDataAccess
    {
        #region 基础方法
        // Sqlite的数据处理对象
        private SQLiteConnection conn = null;// 建立连接
        private SQLiteCommand comm = null;// 执行SQL
        private SQLiteDataAdapter adapter = null;// 填充数据列表
        private SQLiteTransaction trans = null;//事务

        // Sqlite数据库的连接字符串
        string connStr = "Data Source=data.db;Version=3;";
        /// <summary>
        /// 执行一个SQL语句，并将结果通过DataTable返回
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataTable GetDatas(string sql, Dictionary<string, object> param = null)
        {
            try
            {
                if (conn == null)
                    conn = new SQLiteConnection(connStr);
                conn.Open();

                adapter = new SQLiteDataAdapter(sql, conn);

                if (param != null)
                {
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                    foreach (var item in param)
                    {
                        parameters.Add(
                            new SQLiteParameter(item.Key, DbType.String)
                            { Value = item.Value }
                        );
                        adapter.SelectCommand.Parameters.Add(new SQLiteParameter(item.Key, DbType.String)
                        { Value = item.Value });
                    }
                    //adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                }
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                this.Dispose();
            }
        }
        private int SqlExecute(List<string> sqls)
        {
            int count = 0;
            try
            {
                if (conn == null)
                    conn = new SQLiteConnection(connStr);
                conn.Open();

                trans = conn.BeginTransaction();
                foreach (var sql in sqls)
                {
                    comm = new SQLiteCommand(sql, conn);
                    count += comm.ExecuteNonQuery();
                }
                trans.Commit();
                return count;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

        }
        private void Dispose()
        {
            if (trans != null)
            {
                trans.Dispose();
                trans = null;
            }
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }
        #endregion


        #region 登录逻辑
        public DataTable Login(string username, string password)
        {
            // 不能拼接 ，Sql注入攻击
            string userSql = "select * from sys_users where user_name=@user_name and password=@password";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@user_name", username);
            param.Add("@password", password);

            DataTable dataTable = this.GetDatas(userSql, param);
            if (dataTable.Rows.Count == 0)
                throw new Exception("用户名或密码错误");

            return dataTable;
        }
        #endregion

        /// <summary>
        /// 获取数据库中的自定义组件
        /// </summary>
        /// <returns></returns>
        public List<DeviceEntity> GetDevices()
        {
            string sql = "select * from devices";
            DataTable dt = this.GetDatas(sql, null);
            var result = dt.AsEnumerable().Select(d =>
            {
                return new DeviceEntity()
                {
                    DeviceNum = d["d_num"].ToString(),
                    X = d["x"].ToString(),
                    Y = d["y"].ToString(),
                    Z = d["z"].ToString(),
                    W = d["w"].ToString(),
                    H = d["h"].ToString(),
                    DeviceTypeName = d["d_type_name"].ToString(),
                    Header = d["header"].ToString()
                };
            });

            return result.ToList();
        }
        public int SaveDevice(List<DeviceEntity> devices)
        {
            try
            {
                int count = 0;
                conn = new SQLiteConnection(connStr);
                conn.Open();
                trans = conn.BeginTransaction();

                string sql = $"delete from devices";

                comm = new SQLiteCommand(sql, conn);
                comm.ExecuteNonQuery();//删除

                foreach (var item in devices)
                {
                    sql = $"insert into devices(d_num,x,y,z,w,h,d_type_name,header,flow_direction,rotate) values('{item.DeviceNum}','{item.X}','{item.Y}','{item.Z}','{item.W}','{item.H}','{item.DeviceTypeName}','{item.Header}','{item.FlowDirection}','{item.Rotate}')";
                    comm.CommandText = sql;
                    var flag = comm.ExecuteNonQuery();// 插入

                    count += flag;

                    // 保存对应的属性
                    // 属性
                    sql = $"delete from device_properties where d_num={item.DeviceNum}";
                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();
                    foreach (var p in item.Props)
                    {
                        if (string.IsNullOrEmpty(p.PropName) || string.IsNullOrEmpty(p.PropValue)) continue;

                        sql = $"insert into device_properties(d_num,prop_name,prop_value) values('{item.DeviceNum}','{p.PropName}','{p.PropValue}')";
                        comm.CommandText = sql;
                        comm.ExecuteNonQuery();// 插入
                    }
                    // 保存对应的点位信息
                    // 清除点位前,需要将报警条件\联控条件清除,再重新添加
                    sql = $"delete from variables where d_num={item.DeviceNum}";
                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();
                    foreach (var v in item.Vars)
                    {
                        if (string.IsNullOrEmpty(v.Header) || string.IsNullOrEmpty(v.Address)) continue;

                        sql = $"insert into variables(d_num,v_num,header,address,offset,modulus) values('{item.DeviceNum}','{v.VarNum}','{v.Header}','{v.Address}','{v.Offset}','{v.Modulus}')";
                        comm.CommandText = sql;
                        comm.ExecuteNonQuery();// 插入
                    }
                    // 对应的报警  需要同步删除
                }
                trans.Commit();
                return count;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<PropEntity> GetPropertyOption()
        {
            throw new NotImplementedException();
        }

        public List<ThumbEntity> GetThumbList()
        {
            throw new NotImplementedException();
        }
        /* public int SaveDevice(List<DeviceEntity> devices)
{
    try
    {
        int count = 0;
        conn = new SQLiteConnection(connStr);
        conn.Open();
        trans = conn.BeginTransaction();

        string sql = $"delete from devices";

        comm = new SQLiteCommand(sql, conn);
        comm.ExecuteNonQuery();//删除

        foreach (var item in devices)
        {
            sql = $"insert into devices(d_num,x,y,z,w,h,d_type_name,header,flow_direction,rotate) values('{item.DeviceNum}','{item.X}','{item.Y}','{item.Z}','{item.W}','{item.H}','{item.DeviceTypeName}','{item.Header}','{item.FlowDirection}','{item.Rotate}')";
            comm.CommandText = sql;
            var flag = comm.ExecuteNonQuery();// 插入

            count += flag;

            // 保存对应的属性
            // 属性
            sql = $"delete from device_properties where d_num={item.DeviceNum}";
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            foreach (var p in item.Props)
            {
                if (string.IsNullOrEmpty(p.PropName) || string.IsNullOrEmpty(p.PropValue)) continue;

                sql = $"insert into device_properties(d_num,prop_name,prop_value) values('{item.DeviceNum}','{p.PropName}','{p.PropValue}')";
                comm.CommandText = sql;
                comm.ExecuteNonQuery();// 插入
            }
            // 保存对应的点位信息
            // 清除点位前,需要将报警条件\联控条件清除,再重新添加
            sql = $"delete from variables where d_num={item.DeviceNum}";
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
            foreach (var v in item.Vars)
            {
                if (string.IsNullOrEmpty(v.Header) || string.IsNullOrEmpty(v.Address)) continue;

                sql = $"insert into variables(d_num,v_num,header,address,offset,modulus) values('{item.DeviceNum}','{v.VarNum}','{v.Header}','{v.Address}','{v.Offset}','{v.Modulus}')";
                comm.CommandText = sql;
                comm.ExecuteNonQuery();// 插入
            }
            // 对应的报警  需要同步删除
        }
        trans.Commit();
        return count;
    }
    catch (Exception ex)
    {
        trans.Rollback();
        throw ex;
    }
    finally
    {
        this.Dispose();
    }
}

public List<DeviceEntity> GetDevices()
{
    string sql = "select * from devices";
    DataTable dt = this.GetDatas(sql, null);
    var result = dt.AsEnumerable().Select(d =>
    {
        return new DeviceEntity()
        {
            DeviceNum = d["d_num"].ToString(),
            X = d["x"].ToString(),
            Y = d["y"].ToString(),
            Z = d["z"].ToString(),
            W = d["w"].ToString(),
            H = d["h"].ToString(),
            DeviceTypeName = d["d_type_name"].ToString(),
            Header = d["header"].ToString()
        };
    });

    return result.ToList();
}

public List<ThumbEntity> GetThumbList()
{
    string sql = "select * from thumbs";
    DataTable dt = this.GetDatas(sql, null);

    var result = from d in dt.AsEnumerable()
                 select new ThumbEntity
                 {
                     Icon = d["icon"].ToString(),
                     Header = d["header"].ToString(),
                     TargetType = d["target_type"].ToString(),
                     Width = int.Parse(d["w"].ToString()),
                     Height = int.Parse(d["h"].ToString()),
                     Category = d["category"].ToString()
                 };

    return result.ToList();
}

public List<PropEntity> GetPropertyOption()
{
    string sql = "select * from properties";
    DataTable dt1 = this.GetDatas(sql, null);

    var result = (from q1 in dt1.AsEnumerable()
                  select new PropEntity
                  {
                      PropHeader = q1["p_header"].ToString(),
                      PropName = q1["p_name"].ToString(),
                      PropType = int.Parse(q1["p_type"].ToString())
                  }).ToList();

    return result;
}*/
    }

}
