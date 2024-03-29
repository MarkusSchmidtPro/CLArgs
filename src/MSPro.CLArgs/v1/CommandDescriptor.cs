﻿using System;
using System.Diagnostics.CodeAnalysis;



namespace MSPro.CLArgs
{
    /// <summary>
    /// </summary>
    public class CommandDescriptor
    {
        /// <summary>
        /// </summary>
        /// <param name="verb"></param>
        /// <param name="factoryFunc"></param>
        /// <param name="description"></param>
        public CommandDescriptor([NotNull] string verb, [NotNull] Func<ICommand> factoryFunc, string description = null)
        {
            Verb                  = verb;
            CreateCommandInstance = factoryFunc;
            Description           = description;
        }



        /// <summary>
        /// </summary>
        public string Verb { get; }

        /// <summary>
        /// </summary>
        public Func<ICommand> CreateCommandInstance { get; }

        /// <summary>
        /// </summary>
        public string Description { get; }
    }
}