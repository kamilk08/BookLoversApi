using System;
using System.Collections.Generic;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class ChangeBookcaseOptionsWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public List<SelectedOptionWriteModel> SelectedOptions { get; set; }
    }
}