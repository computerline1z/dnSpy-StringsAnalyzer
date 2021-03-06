﻿using System.Collections.Generic;
using System.Windows.Controls;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

using static Plugin.StringAnalyzer.ToolWindowVM;
using System;
using System.Windows;

namespace Plugin.StringAnalyzer {
	public partial class ToolWindowControl : UserControl {
		public ToolWindowControl() {
			InitializeComponent();
    }
        public static List<MethodDef> method = new List<MethodDef>();
        public static List<stringAnalyzerData> items = new List<stringAnalyzerData>();
        public static string filename = "dnSpy.StringsAnalyzer.x.dll"; // Demonstration purposes only
        // as I'm going to later use the currently inspected assembly from the navigation and not 
        // loading it individually.
        public static int i;
        public static ContextMenu ContextMenu1;

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            items.Clear();
            ModuleDefMD md = ModuleDefMD.Load(filename);
            foreach (TypeDef types in md.Types)
            {
                foreach (MethodDef mdInfo in types.Methods)
                {
                    i++;
                    if (mdInfo.HasBody)
                    {
                        //var dict = new Dictionary<keytype, valuetype>();
                        var instr = mdInfo.Body.Instructions;

                        foreach (var iInstr in instr)
                        {
                            MDToken token = mdInfo.MDToken;

                            /* 
                             * Many thanks to Mr.Exodia for helping me out on this part below \(^o^)/~ 
                             */

                            if (iInstr.OpCode.Equals(OpCodes.Ldstr))
                            {
                                string formattedOffset = string.Format("IL_{0:X4}", iInstr.GetOffset());

                                items.Add(new stringAnalyzerData()
                                {
                                    stringValue = iInstr.GetOperand().ToString(),
                                    ilOffset = formattedOffset,
                                    mdToken = string.Format("0x{0:x}", token.ToString().Remove(0, 1)),
                                    mdName = mdInfo.Name,
                                    fullmdName = mdInfo.FullName,
                                });
                            }
                        }
                    }
                    
                }
            }
            lvStringsAnalyzer.ItemsSource = items;
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            items.Clear();
        }
    }
}
