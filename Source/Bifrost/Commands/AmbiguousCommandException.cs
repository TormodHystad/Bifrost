﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bifrost.Commands
{
    /// <summary>
    /// The exception that is thrown if two commands have the same type name
    /// </summary>
    public class AmbiguousCommandException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AmbiguousCommandException"/>
        /// </summary>
        /// <param name="first">The existing command - first discovered</param>
        /// <param name="second">The second command discovered that has the same name</param>
        public AmbiguousCommandException(Type first, Type second) 
            : base
                (
                    string.Format
                        ("Command '{0}' has the same name as '{1}', names must be unique across an application",
                            second.AssemblyQualifiedName, first.AssemblyQualifiedName
                        )
                ) { }
    }
}
