﻿using Talabat.APIs.Dtos;

namespace Talabat.APIs.Helpers
{
    //Standard response to any endpoint work with Pagination
    public class Pagination<T>
    {

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }


        public Pagination(int pageIndex, int pageSize, int count,IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
           Count = count;
            Data = data;
        }

    }
}
