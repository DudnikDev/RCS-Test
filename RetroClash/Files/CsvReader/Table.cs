using System;
using System.Collections.Generic;
using System.IO;

namespace RetroClash.Files.CsvReader
{
    public class Table
    {
        private readonly List<Column> _columns;
        private readonly List<string> _headers;
        private readonly List<Row> _rows;
        private readonly List<string> _types;

        public Table(string path)
        {
            try
            {
                _rows = new List<Row>();
                _headers = new List<string>();
                _types = new List<string>();
                _columns = new List<Column>();

                using (var reader = new StreamReader(path))
                {
                    var columns = reader.ReadLine()?.Replace("\"", string.Empty).Replace(" ", string.Empty).Split(',');
                    if (columns != null)
                        foreach (var column in columns)
                        {
                            _headers.Add(column);
                            _columns.Add(new Column());
                        }

                    var types = reader.ReadLine()?.Replace("\"", string.Empty).Split(',');
                    if (types != null)
                        foreach (var type in types)
                            _types.Add(type);

                    while (!reader.EndOfStream)
                    {
                        var values = reader.ReadLine()?.Replace("\"", string.Empty).Split(',');

                        if (values != null && !string.IsNullOrEmpty(values[0]))
                            new Row(this);

                        for (var i = 0; i < _headers.Count; i++)
                            if (values != null) _columns[i].Add(values[i]);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void AddRow(Row row)
        {
            _rows.Add(row);
        }

        public int GetArraySizeAt(Row row, int columnIndex)
        {
            var index = _rows.IndexOf(row);
            if (index == -1)
                return 0;

            var c = _columns[columnIndex];
            int nextOffset;
            if (index + 1 >= _rows.Count)
            {
                nextOffset = c.GetSize();
            }
            else
            {
                var nextRow = _rows[index + 1];
                nextOffset = nextRow.GetRowOffset();
            }

            return Column.GetArraySize(row.GetRowOffset(), nextOffset);
        }

        public int GetColumnIndexByName(string name)
        {
            return _headers.IndexOf(name);
        }

        public string GetColumnName(int index)
        {
            return _headers[index];
        }

        public int GetColumnRowCount()
        {
            if (_columns.Count > 0)
                return _columns[0].GetSize();

            return 0;
        }

        public Row GetRowAt(int index)
        {
            return _rows[index];
        }

        public int GetRowCount()
        {
            return _rows.Count;
        }

        public string GetValue(string name, int level)
        {
            return GetValueAt(_headers.IndexOf(name), level);
        }

        public string GetValueAt(int column, int row)
        {
            return _columns[column].Get(row);
        }
    }
}