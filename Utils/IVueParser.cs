using System;
using System.Collections.Generic;

namespace ECSForm.Utils
{
    public interface IVueParser
    {
        Dictionary<string, object?> ParseData<TModel>(TModel model);
    }
}