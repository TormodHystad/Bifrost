﻿#region License
//
// Copyright (c) 2008-2012, DoLittle Studios AS and Komplett ASA
//
// Licensed under the Microsoft Permissive License (Ms-PL), Version 1.1 (the "License")
// With one exception :
//   Commercial libraries that is based partly or fully on Bifrost and is sold commercially,
//   must obtain a commercial license.
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at
//
//   http://bifrost.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using Bifrost.Events;
using Bifrost.Sagas;
using Bifrost.Execution;

namespace Bifrost.Commands
{
    /// <summary>
    /// Represents a <see cref="ICommandContextManager">Command context manager</see>
    /// </summary>
    public class CommandContextManager : ICommandContextManager
    {
        readonly IEventStore _eventStore;
        readonly ISagaLibrarian _sagaLibrarian;
        readonly IProcessMethodInvoker _processMethodInvoker;
        readonly IExecutionContextManager _executionContextManager;

        [ThreadStatic] static ICommandContext _currentContext;

        /// <summary>
        /// Reset context
        /// </summary>
        public static void ResetContext()
        {
            _currentContext = null;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandContextManager">CommandContextManager</see>
        /// </summary>
        /// <param name="eventStore">A <see cref="IEventStore">IEventStore</see> to use for saving events</param>
        /// <param name="sagaLibrarian">A <see cref="ISagaLibrarian"/> for saving sagas to</param>
        /// <param name="processMethodInvoker">A <see cref="IProcessMethodInvoker"/> for processing events</param>
        /// <param name="executionContextManager">A <see cref="IExecutionContextManager"/> for getting execution context from</param>
        public CommandContextManager(
            IEventStore eventStore,
            ISagaLibrarian sagaLibrarian,
            IProcessMethodInvoker processMethodInvoker,
            IExecutionContextManager executionContextManager)
        {
            _eventStore = eventStore;
            _sagaLibrarian = sagaLibrarian;
            _processMethodInvoker = processMethodInvoker;
            _executionContextManager = executionContextManager;
        }

        private static bool IsInContext(ICommand command)
        {
            var inContext = null != _currentContext && _currentContext.Command.Equals(command);
            return inContext;
        }

#pragma warning disable 1591 // Xml Comments
        public bool HasCurrent
        {
            get { return _currentContext != null; }
        }

        public ICommandContext GetCurrent()
        {
            if (!HasCurrent)
            {
                throw new InvalidOperationException(ExceptionStrings.CommandNotEstablished);
            }
            return _currentContext;
        }

        public ICommandContext EstablishForCommand(ICommand command)
        {
            if (!IsInContext(command))
            {
                var commandContext = new CommandContext(
                    command,
                    _executionContextManager.Current,
                    _eventStore
                    );

                _currentContext = commandContext;
            }
            return _currentContext;
        }

        public ICommandContext EstablishForSaga(ISaga saga, ICommand command)
        {
            if (!IsInContext(command))
            {
                var commandContext = new SagaCommandContext(
                        saga,
                        command,
                        _executionContextManager.Current,
                        _eventStore,
                        _processMethodInvoker,
                        _sagaLibrarian
                    );

                _currentContext = commandContext;
            }
            return _currentContext;
        }
#pragma warning restore 1591 // Xml Comments
    }
}