// Copyright (c) 2008-2015, Hazelcast, Inc. All Rights Reserved.
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

using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class ClientAuthenticationCodec
    {
        public const int ResponseType = 107;
        public const bool Retryable = true;

        public static readonly ClientMessageType RequestType = ClientMessageType.ClientAuthentication;

        public static ResponseParameters DecodeResponse(IClientMessage clientMessage)
        {
            var parameters = new ResponseParameters();
            byte status;
            status = clientMessage.GetByte();
            parameters.status = status;
            Address address = null;
            var address_isNull = clientMessage.GetBoolean();
            if (!address_isNull)
            {
                address = AddressCodec.Decode(clientMessage);
                parameters.address = address;
            }
            string uuid = null;
            var uuid_isNull = clientMessage.GetBoolean();
            if (!uuid_isNull)
            {
                uuid = clientMessage.GetStringUtf8();
                parameters.uuid = uuid;
            }
            string ownerUuid = null;
            var ownerUuid_isNull = clientMessage.GetBoolean();
            if (!ownerUuid_isNull)
            {
                ownerUuid = clientMessage.GetStringUtf8();
                parameters.ownerUuid = ownerUuid;
            }
            byte serializationVersion;
            serializationVersion = clientMessage.GetByte();
            parameters.serializationVersion = serializationVersion;
            return parameters;
        }

        public static ClientMessage EncodeRequest(string username, string password, string uuid, string ownerUuid,
            bool isOwnerConnection, string clientType, byte serializationVersion)
        {
            var requiredDataSize = RequestParameters.CalculateDataSize(username, password, uuid, ownerUuid,
                isOwnerConnection, clientType, serializationVersion);
            var clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int) RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(username);
            clientMessage.Set(password);
            bool uuid_isNull;
            if (uuid == null)
            {
                uuid_isNull = true;
                clientMessage.Set(uuid_isNull);
            }
            else
            {
                uuid_isNull = false;
                clientMessage.Set(uuid_isNull);
                clientMessage.Set(uuid);
            }
            bool ownerUuid_isNull;
            if (ownerUuid == null)
            {
                ownerUuid_isNull = true;
                clientMessage.Set(ownerUuid_isNull);
            }
            else
            {
                ownerUuid_isNull = false;
                clientMessage.Set(ownerUuid_isNull);
                clientMessage.Set(ownerUuid);
            }
            clientMessage.Set(isOwnerConnection);
            clientMessage.Set(clientType);
            clientMessage.Set(serializationVersion);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly ClientMessageType TYPE = RequestType;
            public string clientType;
            public bool isOwnerConnection;
            public string ownerUuid;
            public string password;
            public byte serializationVersion;
            public string username;
            public string uuid;

            public static int CalculateDataSize(string username, string password, string uuid, string ownerUuid,
                bool isOwnerConnection, string clientType, byte serializationVersion)
            {
                var dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(username);
                dataSize += ParameterUtil.CalculateDataSize(password);
                dataSize += Bits.BooleanSizeInBytes;
                if (uuid != null)
                {
                    dataSize += ParameterUtil.CalculateDataSize(uuid);
                }
                dataSize += Bits.BooleanSizeInBytes;
                if (ownerUuid != null)
                {
                    dataSize += ParameterUtil.CalculateDataSize(ownerUuid);
                }
                dataSize += Bits.BooleanSizeInBytes;
                dataSize += ParameterUtil.CalculateDataSize(clientType);
                dataSize += Bits.ByteSizeInBytes;
                return dataSize;
            }
        }

        //************************ RESPONSE *************************//


        public class ResponseParameters
        {
            public Address address;
            public string ownerUuid;
            public byte serializationVersion;
            public byte status;
            public string uuid;
        }
    }
}