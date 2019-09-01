﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ApplicationCore.SqlScripts.SqlServer
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class ScrambleTable : ScrambleTableBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\nalter table ");
            
            #line 7 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" disable trigger all; \r\n\r\ncreate table #prim_keys_and_columns\r\n(\r\n  ");
            
            #line 11 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
  for(int i=0;i<TableInfo.PrimaryKeysAndTypes.Count;i++) 
      {
        var columnName = TableInfo.PrimaryKeysAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}] {TableInfo.PrimaryKeysAndTypes[columnName]}, ");                      
      }
 
      for(int i=0;i<TableInfo.SoloScrambledColumnsAndTypes.Count;i++) 
      {
        var columnName = TableInfo.SoloScrambledColumnsAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}] {TableInfo.SoloScrambledColumnsAndTypes[columnName]}, ");              
      }
    
     for(int i=0;i<TableInfo.PairedColumnsInside.Count;i++) 
      {
        var pairedColumns = TableInfo.PairedColumnsInside[i];
        for(int j = 0; j<pairedColumns.Count;j++) 
        {
            var columnName = pairedColumns.Keys.ElementAt(j);
            Write($"[{columnName}] {pairedColumns[columnName]}, ");
        }   
      }
    
            
            #line default
            #line hidden
            this.Write(" \r\n    rownum int\r\n);\r\n\r\n");
            
            #line 36 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
   
    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++, _scrambleTableNumber++) 
    {
        var columnName = TableInfo.SoloScrambledColumnsAndTypes.Keys.ElementAt(i);

            
            #line default
            #line hidden
            this.Write("create table #column");
            
            #line 41 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_scrambleTableNumber));
            
            #line default
            #line hidden
            this.Write("\r\n(   \r\n  random int,   \r\n  [");
            
            #line 44 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnName));
            
            #line default
            #line hidden
            this.Write("] ");
            
            #line 44 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.SoloScrambledColumnsAndTypes[columnName]));
            
            #line default
            #line hidden
            this.Write("     \r\n);\r\n");
            
            #line 46 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            
            #line 51 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
   
    for(int i = 0; i < TableInfo.PairedColumnsInside.Count; i++, _scrambleTableNumber++) 
    {
        var pairedColumns = TableInfo.PairedColumnsInside[i];

            
            #line default
            #line hidden
            this.Write("create table #column");
            
            #line 56 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_scrambleTableNumber));
            
            #line default
            #line hidden
            this.Write("\r\n(  \r\n  random int, \r\n ");
            
            #line 59 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
  
    for(int j =0; j < pairedColumns.Keys.Count; j++) 
    { 
     var columnName = pairedColumns.Keys.ElementAt(j);
     Write($"[{columnName}] {pairedColumns[columnName]}");

     if (j != pairedColumns.Keys.Count-1)
     {
       Write(", ");
     }
    } 
    
            
            #line default
            #line hidden
            this.Write("               \r\n);\r\n");
            
            #line 72 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\ninsert into #prim_keys_and_columns with (tablock)\r\n    select ");
            
            #line 77 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 
    foreach(var columnName in TableInfo.PrimaryKeysAndTypes.Keys) 
    {
        Write($"[{columnName}], ");
    }

    foreach(var columnName in TableInfo.SoloScrambledColumnsAndTypes.Keys) 
    {
        Write($"[{columnName}], ");
    }

    foreach(var pairedColumns in TableInfo.PairedColumnsInside) 
    {   
        foreach(var columnName in pairedColumns.Keys)
        {
            Write($"[{columnName}], ");
        }
    }    

            
            #line default
            #line hidden
            this.Write(" row_number() over (order by \r\n    ");
            
            #line 96 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    for(int i = 0; i < TableInfo.PrimaryKeysAndTypes.Count; i++)
    {   
        var columnName= TableInfo.PrimaryKeysAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}]");

        if (i != (TableInfo.PrimaryKeysAndTypes.Count - 1))
        {
            Write(", ");
        }
    }
    
            
            #line default
            #line hidden
            this.Write(" )\r\n    from ");
            
            #line 108 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\n\r\n");
            
            #line 111 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
    _scrambleTableNumber = 0;
    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++, _scrambleTableNumber++) 
    {
        var columnName = TableInfo.SoloScrambledColumnsAndTypes.Keys.ElementAt(i);

            
            #line default
            #line hidden
            this.Write("insert into #column");
            
            #line 116 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_scrambleTableNumber));
            
            #line default
            #line hidden
            this.Write(" with (tablock)\r\n    select row_number() over (order by x), [");
            
            #line 117 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnName));
            
            #line default
            #line hidden
            this.Write("] from \r\n    (select CHECKSUM(NewId()) x, [");
            
            #line 118 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(columnName));
            
            #line default
            #line hidden
            this.Write("] from #prim_keys_and_columns) a;\r\n");
            
            #line 119 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            
            #line 124 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
   
    for(int i = 0; i < TableInfo.PairedColumnsInside.Count; i++, _scrambleTableNumber++) 
    {
        var pairedColumns = TableInfo.PairedColumnsInside[i];

            
            #line default
            #line hidden
            this.Write("insert into #column");
            
            #line 129 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_scrambleTableNumber));
            
            #line default
            #line hidden
            this.Write(" with (tablock)\r\n    select row_number() over (order by x), ");
            
            #line 130 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
  
        for(int j = 0; j < pairedColumns.Count; j++) 
        { 
            var columnName = pairedColumns.Keys.ElementAt(j);
            Write($"[{columnName}]");
            
            if (j != (pairedColumns.Count-1))
            {
                Write(", ");
            }
        } 
    
            
            #line default
            #line hidden
            this.Write("    \r\n    from (select CHECKSUM(NewId()) x, ");
            
            #line 142 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
  
        for(int j = 0; j < pairedColumns.Count; j++) 
        { 
            var columnName = pairedColumns.Keys.ElementAt(j);
            Write($"[{columnName}]");
            
            if (j != (pairedColumns.Count-1))
            {
                Write(", ");
            }
        } 
    
            
            #line default
            #line hidden
            this.Write(" from #prim_keys_and_columns) a;             \r\n");
            
            #line 154 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\n\r\ncreate index idx_primarykey on #prim_keys_and_columns(  ");
            
            #line 159 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    for(int i = 0; i < TableInfo.PrimaryKeysAndTypes.Count; i++)
    {   
        var columnName= TableInfo.PrimaryKeysAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}]");

        if (i != (TableInfo.PrimaryKeysAndTypes.Count - 1))
        {
            Write(", ");
        }
    }
    
            
            #line default
            #line hidden
            this.Write(" );\r\n\r\n");
            
            #line 172 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    for(int i = 0; i < _scrambleTableNumber; i++)
    {
        Write($"create index idx_column{i}_random on #column{i}(random);");
        Write(Environment.NewLine);
    }

            
            #line default
            #line hidden
            this.Write("\r\nalter table ");
            
            #line 180 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" nocheck constraint all;\r\n\r\nupdate ");
            
            #line 182 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" with (tablock) \r\nset ");
            
            #line 183 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++)
    {   
        var columnName= TableInfo.SoloScrambledColumnsAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}] = x.[{columnName}]");

        if ((i == (TableInfo.SoloScrambledColumnsAndTypes.Count - 1)) &&
            (TableInfo.PairedColumnsInside.Count == 0) && 
            (TableInfo.ConstantColumnsAndTypesAndValues.Count == 0) )
        {}
        else 
        { 
            Write(", ");
        }      
    }
    
    for(int i=0;i<TableInfo.PairedColumnsInside.Count;i++) 
    {
        var pairedColumns = TableInfo.PairedColumnsInside[i];
        for(int j = 0; j < pairedColumns.Count;j++) 
        {
            var columnName = pairedColumns.Keys.ElementAt(j);
            Write($"[{columnName}] = x.[{columnName}]");

            if ((j == (pairedColumns.Count - 1)) && (i == TableInfo.PairedColumnsInside.Count - 1) &&
                (TableInfo.ConstantColumnsAndTypesAndValues.Count == 0) )
                {}
            else 
            { 
                Write(", ");
            } 
        }   
    }

    for(int i=0;i<TableInfo.ConstantColumnsAndTypesAndValues.Count;i++) 
    {
        var column = TableInfo.ConstantColumnsAndTypesAndValues[i];
        Write($"[{column.Name}] = cast('{column.Value}' as {column.Type})");                      
    }
    
            
            #line default
            #line hidden
            this.Write(" from\r\n(\r\n    select ");
            
            #line 224 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
    var scrambleTableNumberIter= 0;
    for(int i = 0; i < TableInfo.PrimaryKeysAndTypes.Count; i++)
    {   
        var columnName= TableInfo.PrimaryKeysAndTypes.Keys.ElementAt(i);
        Write($"[{columnName}], ");
    }
    
    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++, scrambleTableNumberIter++)
    {   
        var columnName= TableInfo.SoloScrambledColumnsAndTypes.Keys.ElementAt(i);
        Write($"#column{scrambleTableNumberIter}.[{columnName}]");

        if ((i == (TableInfo.SoloScrambledColumnsAndTypes.Count - 1)) &&
            (TableInfo.PairedColumnsInside.Count == 0))
        {}
        else 
        { 
            Write(", ");
        }      
    }

    for(int i=0;i<TableInfo.PairedColumnsInside.Count;i++, scrambleTableNumberIter++) 
    {
        var pairedColumns = TableInfo.PairedColumnsInside[i];
        for(int j = 0; j < pairedColumns.Count;j++) 
        {
            var columnName = pairedColumns.Keys.ElementAt(j);
            Write($"#column{scrambleTableNumberIter}.[{columnName}]");

            if ((j == (pairedColumns.Count - 1)) && (i == TableInfo.PairedColumnsInside.Count - 1))
                {}
            else 
            { 
                Write(", ");
            } 
        }   
    }
    
            
            #line default
            #line hidden
            this.Write(" from #prim_keys_and_columns\r\n    ");
            
            #line 262 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 scrambleTableNumberIter = 0;
    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++, scrambleTableNumberIter++)
    {   
        Write($"join #column{scrambleTableNumberIter} on #prim_keys_and_columns.rownum = #column{scrambleTableNumberIter}.random");
        Write(Environment.NewLine);     
    }

    for(int i=0;i<TableInfo.PairedColumnsInside.Count;i++, scrambleTableNumberIter++) 
    {
       Write($"join #column{scrambleTableNumberIter} on #prim_keys_and_columns.rownum = #column{scrambleTableNumberIter}.random");  
       Write(Environment.NewLine);
    }
    
            
            #line default
            #line hidden
            this.Write(") x\r\nwhere ");
            
            #line 276 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 for(int i = 0; i < TableInfo.PrimaryKeysAndTypes.Count; i++)
    {   
        var columnName = TableInfo.PrimaryKeysAndTypes.Keys.ElementAt(i);
        Write($"{TableInfo.FullTableName}.[{columnName}] = x.[{columnName}]");

        if (i != (TableInfo.PrimaryKeysAndTypes.Count - 1))
        {
            Write(", ");
        }
    } 
            
            #line default
            #line hidden
            this.Write(" ;\r\n\r\ndrop table #prim_keys_and_columns;\r\n");
            
            #line 288 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 scrambleTableNumberIter = 0;
    for(int i = 0; i < TableInfo.SoloScrambledColumnsAndTypes.Count; i++, scrambleTableNumberIter++)
    {   
        Write($"drop table #column{scrambleTableNumberIter};");
        Write(Environment.NewLine);     
    }

    for(int i=0;i<TableInfo.PairedColumnsInside.Count;i++, scrambleTableNumberIter++) 
    {
       Write($"drop table #column{scrambleTableNumberIter};");  
       Write(Environment.NewLine);
    }
    
            
            #line default
            #line hidden
            this.Write("\r\nalter table ");
            
            #line 302 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" enable trigger all;\r\nalter table ");
            
            #line 303 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" check constraint all;\r\n\r\n\r\n");
            
            #line 306 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 
//if there is more than one mapped table
    foreach(var mappedTable in TableInfo.MappedTablesOutside.Where(m => m.MappedColumnPairsOutside.Count > 1))
    { 

        var lastMappingStep = mappedTable.MappedColumnPairsOutside.Last();
    
            
            #line default
            #line hidden
            this.Write("update  ");
            
            #line 313 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(lastMappingStep.DestinationTableNameWithSchema));
            
            #line default
            #line hidden
            this.Write("\r\nset ");
            
            #line 314 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 
        for(int i = 0; i < mappedTable.SourceDestPairedColumnsOutside.Count; i++)
        {
            var mappedColumn = mappedTable.SourceDestPairedColumnsOutside[i];
            Write($"{mappedColumn.SecondColumn} = source.{mappedColumn.FirstColumn}");

            if (i != (mappedTable.SourceDestPairedColumnsOutside.Count -1)) 
            {
                Write($", {Environment.NewLine} ");
            }
        }    
    
            
            #line default
            #line hidden
            this.Write(" \r\nfrom ");
            
            #line 326 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(lastMappingStep.DestinationTableNameWithSchema));
            
            #line default
            #line hidden
            this.Write(" dest\r\n     ");
            
            #line 327 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

        int tableMaxIndexInMapping =  mappedTable.MappedColumnPairsOutside.Count - 1;
        for(int i = (tableMaxIndexInMapping - 1); i >= 0;i--)
        {   
            var mappingStep = mappedTable.MappedColumnPairsOutside[i];

            if (i == (tableMaxIndexInMapping - 1))
            {
                Write($"join {mappingStep.DestinationTableNameWithSchema} a{i} on ");
                for(int j = 0; j < lastMappingStep.MappedColumns.Count;j++)
                {   
                    var columnPair = lastMappingStep.MappedColumns[j];
                    Write($"a{i}.{columnPair.FirstColumn} = dest.{columnPair.SecondColumn}");
                    if (j != (lastMappingStep.MappedColumns.Count - 1))
                    {
                        Write($"and ");
                    }
                    else 
                    {
                        Write(Environment.NewLine);
                    }
                }
            }
            
            //There is more than just 1 mapping table between.
            if ((mappedTable.MappedColumnPairsOutside.Count > 2) && (i != (tableMaxIndexInMapping - 1)))
            {
                Write($"join {mappingStep.DestinationTableNameWithSchema} a{i} on ");
                for(int j = 0; j < mappingStep.MappedColumns.Count;j++)
                {   
                    var columnPair = mappingStep.MappedColumns[j];
                    Write($"a{i+1}.{columnPair.FirstColumn} = a{i}.{columnPair.SecondColumn} ");
                    if (j != (mappingStep.MappedColumns.Count - 1))
                    {
                        Write($"and ");
                    }
                    else 
                    {
                        Write(Environment.NewLine);
                    }
                }
            }

            if(i == 0) 
            {
                Write($"join {TableInfo.FullTableName} source on ");
                for(int j = 0; j < mappingStep.MappedColumns.Count;j++)
                {   
                    var columnPair = mappingStep.MappedColumns[j];
                    Write($"a{i}.{columnPair.SecondColumn} = source.{columnPair.FirstColumn}");
                    if (j != (mappingStep.MappedColumns.Count - 1))
                    {
                        Write($"and ");
                    }
                    else 
                    {
                        Write(";" + Environment.NewLine);
                    }
                }
            }
        }
     
            
            #line default
            #line hidden
            this.Write("   ");
            
            #line 389 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 } 

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 392 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"

    foreach(var mappedTable in TableInfo.MappedTablesOutside.Where(m => m.MappedColumnPairsOutside.Count == 1))
    {
        var lastMappingStep = mappedTable.MappedColumnPairsOutside.Last();
    
            
            #line default
            #line hidden
            this.Write("update  ");
            
            #line 397 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(lastMappingStep.DestinationTableNameWithSchema));
            
            #line default
            #line hidden
            this.Write("    \r\nset ");
            
            #line 398 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
 
        for(int i = 0; i < mappedTable.SourceDestPairedColumnsOutside.Count; i++)
        {
            var mappedColumn = mappedTable.SourceDestPairedColumnsOutside[i];
            Write($"{mappedColumn.SecondColumn} = source.{mappedColumn.FirstColumn}");

            if (i != (mappedTable.SourceDestPairedColumnsOutside.Count -1)) 
            {
                Write($", {Environment.NewLine} ");
            }
        }    
    
            
            #line default
            #line hidden
            this.Write(" \r\nfrom ");
            
            #line 410 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableInfo.FullTableName));
            
            #line default
            #line hidden
            this.Write(" source\r\n  ");
            
            #line 411 "E:\GoogleDrive\Documents\szakdoga\anonymizer\AnonymizerCore\ApplicationCore\SqlScripts\SqlServer\ScrambleTable.tt"
  }

            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public class ScrambleTableBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
