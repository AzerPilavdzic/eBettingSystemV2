﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBettingSystemV2.Models
{
    public static class CacheKeys
    {
        public static string Entry => "_Entry";
        public static string CallbackEntry => "_Callback";
        public static string CallbackMessage => "_CallbackMessage";
        public static string Parent => "_Parent";
        public static string Child => "_Child";
        public static string DependentMessage => "_DependentMessage";
        public static string DependentCTS => "_DependentCTS";
        public static string Ticks => "_Ticks";
        public static string CancelMsg => "_CancelMsg";
        public static string CancelTokenSource => "_CancelTokenSource";
        public static string Podaci => "_Podaci";
        public static string Expire => "_Expire";
    }
}