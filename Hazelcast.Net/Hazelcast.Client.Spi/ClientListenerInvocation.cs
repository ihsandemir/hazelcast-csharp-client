﻿// Copyright (c) 2008-2015, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Hazelcast.Client.Connection;
using Hazelcast.Client.Protocol;
using Hazelcast.IO;
using Hazelcast.Util;

namespace Hazelcast.Client.Spi
{
    internal class ClientListenerInvocation : ClientInvocation
    {
        private readonly DistributedEventHandler _handler;
        private readonly DecodeStartListenerResponse _responseDecoder;

        public ClientListenerInvocation(IClientMessage message, DistributedEventHandler handler,
            DecodeStartListenerResponse responseDecoder)
            : base(message)
        {
            _responseDecoder = responseDecoder;
            _handler = handler;
        }

        public ClientListenerInvocation(IClientMessage message, DistributedEventHandler handler,
            DecodeStartListenerResponse responseDecoder, int partitionId)
            : base(message, partitionId)
        {
            _responseDecoder = responseDecoder;
            _handler = handler;
        }

        public ClientListenerInvocation(IClientMessage message, DistributedEventHandler handler,
            DecodeStartListenerResponse responseDecoder, ClientConnection connection)
            : base(message, connection)
        {
            _responseDecoder = responseDecoder;
            _handler = handler;
        }

        public ClientListenerInvocation(IClientMessage message, DistributedEventHandler handler,
            DecodeStartListenerResponse responseDecoder, string memberUuid)
            : base(message, memberUuid)
        {
            _responseDecoder = responseDecoder;
            _handler = handler;
        }

        public ClientListenerInvocation(IClientMessage message, DistributedEventHandler handler,
            DecodeStartListenerResponse responseDecoder, Address address)
            : base(message, address)
        {
            _responseDecoder = responseDecoder;
            _handler = handler;
        }

        public DistributedEventHandler Handler
        {
            get { return _handler; }
        }

        public DecodeStartListenerResponse ResponseDecoder
        {
            get { return _responseDecoder; }
        }
    }
}