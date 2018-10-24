using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace VidaAnimal.Services
{
  public static class KendoUIQueryHelper
  {
    private static readonly Dictionary<Type, DbType> DbTypeMappings = new Dictionary<Type, DbType>()
        {
            { typeof(byte), DbType.Byte},
            { typeof(sbyte), DbType.Int16},
            { typeof(ushort), DbType.UInt16},
            { typeof(int), DbType.Int32},
            { typeof(uint), DbType.UInt32},
            { typeof(long), DbType.Int64},
            { typeof(ulong), DbType.UInt64 },
            { typeof(float), DbType.Single },
            { typeof(double), DbType.Double},
            { typeof(decimal), DbType.Decimal},
            { typeof(bool), DbType.Boolean},
            { typeof(string), DbType.String },
            { typeof(char), DbType.StringFixedLength},
            { typeof(char[]), DbType.String},
            { typeof(Guid), DbType.Guid},
            { typeof(DateTime), DbType.DateTime},
            { typeof(DateTimeOffset), DbType.DateTimeOffset },
            { typeof(byte[]), DbType.Binary},
            { typeof(byte?), DbType.Byte},
            { typeof(sbyte?), DbType.SByte },
            { typeof(short), DbType.Int16},
            { typeof(short?), DbType.Int16},
            { typeof(ushort?), DbType.UInt16},
            { typeof(int?), DbType.Int32},
            { typeof(uint?), DbType.UInt32},
            { typeof(long?), DbType.Int64},
            { typeof(ulong?), DbType.UInt64},
            { typeof(float?), DbType.Single},
            { typeof(double?), DbType.Double},
            { typeof(decimal?), DbType.Decimal},
            { typeof(bool?), DbType.Boolean},
            { typeof(char?), DbType.StringFixedLength},
            { typeof(Guid?), DbType.Guid},
            { typeof(DateTime?), DbType.DateTime },
            { typeof(DateTimeOffset?), DbType.DateTimeOffset},
            { typeof(TimeSpan), DbType.Time },
            { typeof(TimeSpan?), DbType.Time },
        };

    [Obsolete("Use the overloaded version of this method instead. It's important from a security and performance perspective to always use parameterisation when making database calls.")]
    public static string BuildWhereClause<T>(GridFilters filter)
    {
      var whereclauseBuilder = new StringBuilder();
      if (filter == null || filter.Filters == null || !filter.Filters.Any()) return whereclauseBuilder.ToString();

      filter.Filters.ForEach(f =>
      {
        if (f.Value != null)
        {
          f.Value = f.Value.Replace("_", "[_]");
        }
      });

      var entityType = (typeof(T));
      whereclauseBuilder.Append("WHERE");
      SetPropertyValue<T>(filter.Filters, entityType);
      whereclauseBuilder.Append(filter.MainFieldQuery);
      return whereclauseBuilder.ToString();
    }

    public static string BuildWhereClause<T>(GridFilters filter, out List<SqlParameter> sqlParamList)
    {
      sqlParamList = new List<SqlParameter>();

      var whereclauseBuilder = new StringBuilder();
      if (filter == null || filter.Filters == null || !filter.Filters.Any()) return whereclauseBuilder.ToString();

      filter.Filters.ForEach(f =>
      {
        if (f.Value != null)
        {
          f.Value = f.Value.Replace("_", "[_]");
        }
      });

      var entityType = (typeof(T));
      whereclauseBuilder.Append("WHERE");
      SetPropertyValue<T>(filter.Filters, entityType);
      whereclauseBuilder.Append(filter.GetMainFieldQueryParameterised(sqlParamList, entityType));

      return whereclauseBuilder.ToString();
    }

    public static DbType TypeToDbType(Type t)
    {
      DbType dbType;

      if (!DbTypeMappings.TryGetValue(t, out dbType))
        return DbType.Object;

      return dbType;
    }

    [Obsolete("Reworked to support parameterisation. Latest version is ToSqlOperatorParameterised.")]
    public static string ToSqlOperator(string @operator)
    {
      switch (@operator.ToLower())
      {
        case "eq": return " {1} = '{0}' ";
        case "neq": return " {1} != '{0}' ";
        case "gte": return " {1} >= '{0}' ";
        case "gt": return " {1} > '{0}' ";
        case "lte": return " {1} <= '{0}' ";
        case "lt": return " {1} < '{0}' ";
        case "startswith": return " {1} LIKE '{0}%' ";
        case "endswith": return " {1} LIKE '%{0}' ";
        case "contains": return " {1} LIKE '%{0}%' ";
        case "doesnotcontain": return " {1} NOT LIKE '%{0}%' ";
        default: return "";
      }
    }

    public static string ToSqlOperatorParameterised(string @operator)
    {
      switch (@operator.ToLower())
      {
        case "eq": return " {1} = {0} ";
        case "neq": return " {1} != {0} ";
        case "gte": return " {1} >= {0} ";
        case "gt": return " {1} > {0} ";
        case "lte": return " {1} <= {0} ";
        case "lt": return " {1} < {0} ";
        case "startswith": return " {1} LIKE {0} + '%' ";
        case "endswith": return " {1} LIKE '%' + {0} ";
        case "contains": return " {1} LIKE '%' + {0} + '%' ";
        case "doesnotcontain": return " {1} NOT LIKE '%' + {0} + '%' ";
        default: return string.Empty;
      }
    }

    private static void SetPropertyValue<T>(IEnumerable<GridFilter> filters, Type entityType)
    {
      foreach (var currentFilter in filters)
      {
        if (currentFilter.Field == null)
        {
          SetPropertyValue<T>(currentFilter.Filters, entityType);
          continue;
        }

        var property = GetProperty<T>(currentFilter.Field);

        if (typeof(DateTime).IsAssignableFrom(property.PropertyType) ||
            typeof(DateTime?).IsAssignableFrom(property.PropertyType))
        {
          DateTime dateTimeValue;
          if (DateTime.TryParseExact(Regex.Split(currentFilter.Value, " GMT+")[0], "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
          {
            //currentFilter.Field = String.Format("CAST({0} AS DATE)", currentFilter.Field);
            currentFilter.Value = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
          }
          else if (DateTime.TryParseExact(currentFilter.Value, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
          {
            //currentFilter.Field = String.Format("CAST({0} AS DATE)", currentFilter.Field);
            currentFilter.Value = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
          }
          else
          {
            throw new Exception(String.Format("{0} - Unable to parse Grid {1} DateTime value '{2}'.", entityType.Name, property.Name, currentFilter.Value));
          }
        }
        else if (typeof(TimeSpan).IsAssignableFrom(property.PropertyType) || typeof(TimeSpan?).IsAssignableFrom(property.PropertyType))
        {
          DateTime dateTimeValue;
          if (DateTime.TryParseExact(Regex.Split(currentFilter.Value, " GMT+")[0], "ddd MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
          {
            currentFilter.Value = dateTimeValue.ToString("HH:mm:ss");
          }
          else if (DateTime.TryParseExact(currentFilter.Value, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
          {
            currentFilter.Value = dateTimeValue.ToString("HH:mm:ss");
          }
          else
          {
            throw new Exception(String.Format("{0} - Unable to parse Grid {1} TimeSpan value '{2}'.", entityType.Name, property.Name, currentFilter.Value));
          }
        }
        else if (typeof(int).IsAssignableFrom(property.PropertyType) || typeof(int?).IsAssignableFrom(property.PropertyType))
        {
          decimal decimalValue;
          if (decimal.TryParse(currentFilter.Value, out decimalValue))
          {
            currentFilter.Value = Math.Truncate(decimalValue).ToString(CultureInfo.InvariantCulture);
          }
          else
          {
            throw new Exception(String.Format("{0} - Unable to parse Grid {1} Decimal value '{2}'.", entityType.Name, property.Name, currentFilter.Value));
          }
        }
      }
    }

    [Obsolete("Use generic BuildOrderByClause so that validation can be performed for column name and direction.")]
    public static string BuildOrderByClause(List<GridSort> sorts)
    {
      var orderByclauseBuilder = new StringBuilder();
      if (sorts == null || sorts.Count == 0) return orderByclauseBuilder.ToString();
      orderByclauseBuilder.Append("ORDER BY ");
      orderByclauseBuilder.Append(sorts.Select(x => x.FieldQuery).Aggregate((a, b) => (a + ", " + b)));
      return orderByclauseBuilder.ToString();
    }

    public static string BuildOrderByClause<T>(List<GridSort> sorts)
    {
      var orderByclauseBuilder = new StringBuilder();
      if (sorts == null || sorts.Count == 0) return orderByclauseBuilder.ToString();
      orderByclauseBuilder.Append("ORDER BY ");

      foreach (var sort in sorts)
      {
        // Validates that the sort direction is valid
        if (!string.Equals(sort.dir, "asc", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(sort.dir, "desc", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(sort.dir, "ascending", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(sort.dir, "descending", StringComparison.OrdinalIgnoreCase))
        {

          throw new ArgumentException($"KendoUIQueryHelper query builder. Problem when trying to set direction for column '{sort.field}'. " +
              $"Direction '{sort.dir}' is invalid. Order by direction can only be asc or desc");
        }

        // Validates that the column exists on data contract
        GetProperty<T>(sort.field);
      }

      orderByclauseBuilder.Append(sorts.Select(x => x.FieldQuery).Aggregate((a, b) => (a + ", " + b)));
      return orderByclauseBuilder.ToString();
    }

    public static PropertyInfo GetProperty<T>(string field)
    {
      var entityType = typeof(T);

      var property = entityType.GetProperty(field);

      // Is there a spelling mistake on developers behalf or some dangerous tampering?
      if (property == null)
      {
        throw new ArgumentException($"KendoUIQueryHelper query builder. The property '{field}' does not exist on the model '{entityType?.Name}'. Reflection could not be applied.");
      }

      return property;
    }
  }

  public class GridGroup
  {
    public string Field { get; set; }
    public string Dir { get; set; }
  }

  public class GridSort
  {
    public string field { get; set; }
    public string dir { get; set; }
    public string FieldQuery
    {
      get
      {
        return string.Format("{0} {1}", field, dir);
      }
    }
  }

  public class GridFilter
  {
    public string Operator { get; set; }
    public string Field { get; set; }
    public string Value { get; set; }
    public string Logic { get; set; }

    public List<GridFilter> Filters { get; set; }
    public List<SqlParameter> SqlParameterList { get; set; }

    public string FieldQuery
    {
      get
      {
        if (Filters != null && Filters.Count > 0)
        {
          return Filters.Select(x => x.FieldQuery).Aggregate((a, b) => ("(" + a + " " + Logic.ToUpper() + " " + b + ")"));
        }
        if (string.IsNullOrEmpty(Value))
        {
          return Operator == "neq" ? string.Format("({0} IS NOT NULL)", Field) : string.Format("({0} IS NULL)", Field);
        }
        DateTime parsedVal;
        if (DateTime.TryParse(Value, out parsedVal) && Operator == "eq")
        {
          var dateAtMidnight = new DateTime(parsedVal.Year, parsedVal.Month, parsedVal.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
          var lastMinuteOfDay = new DateTime(parsedVal.Year, parsedVal.Month, parsedVal.Day, 23, 59, 59, 999).ToString("yyyy-MM-dd HH:mm:ss"); ;

          return $"({Field} BETWEEN '{dateAtMidnight}' AND '{lastMinuteOfDay}')";
        }

        return string.Format("(" + KendoUIQueryHelper.ToSqlOperator(Operator) + ")", Value, Field);
      }
    }

    public string GetParameterisedQuery(List<SqlParameter> sqlParamCol, int currCount, Type targetType)
    {
      if (Filters != null && Filters.Count > 0)
      {
        return Filters.Select((x, count) => x.GetParameterisedQuery(sqlParamCol, currCount + count, targetType)).Aggregate((a, b) => ("(" + a + " " + Logic.ToUpper() + " " + b + ")"));
      }
      if (string.IsNullOrEmpty(Value))
      {
        return Operator == "neq" ? string.Format("({0} IS NOT NULL)", Field) : string.Format("({0} IS NULL)", Field);
      }

      DateTime parsedVal;
      if (DateTime.TryParse(Value, out parsedVal) && Operator == "eq")
      {
        var dateAtMidnight = new DateTime(parsedVal.Year, parsedVal.Month, parsedVal.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
        var lastMinuteOfDay = new DateTime(parsedVal.Year, parsedVal.Month, parsedVal.Day, 23, 59, 59, 999).ToString("yyyy-MM-dd HH:mm:ss"); ;

        return $"({Field} BETWEEN '{dateAtMidnight}' AND '{lastMinuteOfDay}')";
      }

      string paramName = $"@{Field}{currCount}";

      var property = targetType.GetProperty(Field);

      DbType dbType = KendoUIQueryHelper.TypeToDbType(property.PropertyType);

      var sqlParam = new SqlParameter();

      sqlParam.ParameterName = paramName;
      sqlParam.Value = (object)Value;
      sqlParam.DbType = dbType;

      if (Nullable.GetUnderlyingType(property.PropertyType) != null)
      {
        // Property is nullable
        sqlParam.IsNullable = true;
      }

      sqlParamCol.Add(sqlParam);

      return string.Format("(" + KendoUIQueryHelper.ToSqlOperatorParameterised(Operator) + ")", paramName, Field);
    }
  }

  public class GridFilters
  {
    public List<GridFilter> Filters { get; set; }
    public string Logic { get; set; }

    public string MainFieldQuery
    {
      get
      {
        if (Filters != null && Filters.Count > 0)
        {
          return string.Format(" ({0}) ", Filters.Select(x => x.FieldQuery).Aggregate((a, b) => (a + " " + Logic.ToUpper() + " " + b)));
        }
        return null;
      }
    }

    public string GetMainFieldQueryParameterised(List<SqlParameter> paramList, Type targetType)
    {
      if (Filters != null && Filters.Count > 0)
      {
        return string.Format(" ({0}) ", Filters.Select((x, y) => x.GetParameterisedQuery(paramList, y + 1, targetType)).Aggregate((a, b) => (a + " " + Logic.ToUpper() + " " + b)));
      }
      return null;
    }
  }
}