using MembershipManager.Engine;
using MembershipManager.DataModel;
using System.Diagnostics.Metrics;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using System.IO.Packaging;
using System.Dynamic;

namespace MembershipManager.DataModel.Person
{
    public class Person
    {
        [DbAttribute("no_avs")]
        public string? NoAvs { get; set; }

        [DbAttribute("first_name")]
        public string? FirstName { get; set; }

        [DbAttribute("last_name")]
        public string? LastName { get; set; }

        [DbAttribute("address")]
        public string? Address { get; set; }

        public int CityId { get; set; }

        [DbRelation("city_id")]
        public City? City { get; set; }


        public static Person? GetPerson(object noAvs)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"SELECT * FROM get_person WHERE no_avs = @value1";
            NpgsqlParameter param = new NpgsqlParameter("@value1", NpgsqlDbType.Char, 13) { Value = noAvs };
            cmd.Parameters.Add(param);

            return DbManager.Db?.Receive<Person>(cmd).First() ?? null;
        }

        public void Insert()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = $"INSERT INTO {this.GetType().Name} {ComputeQuery()}";
            ComputeCommandeWithValues(cmd);
        }


        private string ComputeQuery()   {
            StringBuilder sbAtt = new("(");
            StringBuilder sbVal = new("(");
            int i = 0;
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                DbAttribute? attribute = p.GetCustomAttribute<DbAttribute>();
                if (attribute == null) continue;
                sbAtt.Append(attribute.Name).Append(",");
                sbVal.Append($"value{i++}");

            }

            sbAtt.Append(")");
            sbVal.Append(")");

            return sbAtt.ToString() + " VALUE " + sbVal.ToString();
        }

        private void ComputeCommandeWithValues(NpgsqlCommand cmd)
        {
            int i = 0;
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                DbAttribute? attribute = p.GetCustomAttribute<DbAttribute>();
                if (attribute == null) continue;
                var value = p.GetValue(this);
                var parameterValue1 = cmd.Parameters.Add($"@value{i++}", (NpgsqlDbType)value);
            }
        }
    }
}
