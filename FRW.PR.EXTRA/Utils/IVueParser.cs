using System;
using System.Collections.Generic;

namespace FRW.PR.Extra.Utils
{
    public interface IVueParser
    {
        Dictionary<string, object?> ParseData<TModel>(TModel model);
    }
}