using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpcUaHelper
{
    /// <summary>
    /// A class that defines constants used by UA applications.
    /// 来自UA-NetStandard
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.CodeGenerator", "1.0.0.0")]
    [Serializable]
    public enum OpcUaStatusCodes : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Good = 0x00000000,

        /// <summary>
        /// The operation completed however its outputs may not be usable.
        /// </summary>
        Uncertain = 0x40000000,

        /// <summary>
        /// The operation failed.
        /// </summary>
        Bad = 0x80000000,

        /// <summary>
        /// An unexpected error occurred.
        /// </summary>
        BadUnexpectedError = 0x80010000,

        /// <summary>
        /// An internal error occurred as a result of a programming or configuration error.
        /// </summary>
        BadInternalError = 0x80020000,

        /// <summary>
        /// Not enough memory to complete the operation.
        /// </summary>
        BadOutOfMemory = 0x80030000,

        /// <summary>
        /// An operating system resource is not available.
        /// </summary>
        BadResourceUnavailable = 0x80040000,

        /// <summary>
        /// A low level communication error occurred.
        /// </summary>
        BadCommunicationError = 0x80050000,

        /// <summary>
        /// Encoding halted because of invalid data in the objects being serialized.
        /// </summary>
        BadEncodingError = 0x80060000,

        /// <summary>
        /// Decoding halted because of invalid data in the stream.
        /// </summary>
        BadDecodingError = 0x80070000,

        /// <summary>
        /// The message encoding/decoding limits imposed by the stack have been exceeded.
        /// </summary>
        BadEncodingLimitsExceeded = 0x80080000,

        /// <summary>
        /// The request message size exceeds limits set by the server.
        /// </summary>
        BadRequestTooLarge = 0x80B80000,

        /// <summary>
        /// The response message size exceeds limits set by the client.
        /// </summary>
        BadResponseTooLarge = 0x80B90000,

        /// <summary>
        /// An unrecognized response was received from the server.
        /// </summary>
        BadUnknownResponse = 0x80090000,

        /// <summary>
        /// The operation timed out.
        /// </summary>
        BadTimeout = 0x800A0000,

        /// <summary>
        /// The server does not support the requested service.
        /// </summary>
        BadServiceUnsupported = 0x800B0000,

        /// <summary>
        /// The operation was cancelled because the application is shutting down.
        /// </summary>
        BadShutdown = 0x800C0000,

        /// <summary>
        /// The operation could not complete because the client is not connected to the server.
        /// </summary>
        BadServerNotConnected = 0x800D0000,

        /// <summary>
        /// The server has stopped and cannot process any requests.
        /// </summary>
        BadServerHalted = 0x800E0000,

        /// <summary>
        /// There was nothing to do because the client passed a list of operations with no elements.
        /// </summary>
        BadNothingToDo = 0x800F0000,

        /// <summary>
        /// The request could not be processed because it specified too many operations.
        /// </summary>
        BadTooManyOperations = 0x80100000,

        /// <summary>
        /// The request could not be processed because there are too many monitored items in the subscription.
        /// </summary>
        BadTooManyMonitoredItems = 0x80DB0000,

        /// <summary>
        /// The extension object cannot be (de)serialized because the data type id is not recognized.
        /// </summary>
        BadDataTypeIdUnknown = 0x80110000,

        /// <summary>
        /// The certificate provided as a parameter is not valid.
        /// </summary>
        BadCertificateInvalid = 0x80120000,

        /// <summary>
        /// An error occurred verifying security.
        /// </summary>
        BadSecurityChecksFailed = 0x80130000,

        /// <summary>
        /// The certificate does not meet the requirements of the security policy.
        /// </summary>
        BadCertificatePolicyCheckFailed = 0x81140000,

        /// <summary>
        /// The certificate has expired or is not yet valid.
        /// </summary>
        BadCertificateTimeInvalid = 0x80140000,

        /// <summary>
        /// An issuer certificate has expired or is not yet valid.
        /// </summary>
        BadCertificateIssuerTimeInvalid = 0x80150000,

        /// <summary>
        /// The HostName used to connect to a server does not match a HostName in the certificate.
        /// </summary>
        BadCertificateHostNameInvalid = 0x80160000,

        /// <summary>
        /// The URI specified in the ApplicationDescription does not match the URI in the certificate.
        /// </summary>
        BadCertificateUriInvalid = 0x80170000,

        /// <summary>
        /// The certificate may not be used for the requested operation.
        /// </summary>
        BadCertificateUseNotAllowed = 0x80180000,

        /// <summary>
        /// The issuer certificate may not be used for the requested operation.
        /// </summary>
        BadCertificateIssuerUseNotAllowed = 0x80190000,

        /// <summary>
        /// The certificate is not trusted.
        /// </summary>
        BadCertificateUntrusted = 0x801A0000,

        /// <summary>
        /// It was not possible to determine if the certificate has been revoked.
        /// </summary>
        BadCertificateRevocationUnknown = 0x801B0000,

        /// <summary>
        /// It was not possible to determine if the issuer certificate has been revoked.
        /// </summary>
        BadCertificateIssuerRevocationUnknown = 0x801C0000,

        /// <summary>
        /// The certificate has been revoked.
        /// </summary>
        BadCertificateRevoked = 0x801D0000,

        /// <summary>
        /// The issuer certificate has been revoked.
        /// </summary>
        BadCertificateIssuerRevoked = 0x801E0000,

        /// <summary>
        /// The certificate chain is incomplete.
        /// </summary>
        BadCertificateChainIncomplete = 0x810D0000,

        /// <summary>
        /// User does not have permission to perform the requested operation.
        /// </summary>
        BadUserAccessDenied = 0x801F0000,

        /// <summary>
        /// The user identity token is not valid.
        /// </summary>
        BadIdentityTokenInvalid = 0x80200000,

        /// <summary>
        /// The user identity token is valid but the server has rejected it.
        /// </summary>
        BadIdentityTokenRejected = 0x80210000,

        /// <summary>
        /// The specified secure channel is no longer valid.
        /// </summary>
        BadSecureChannelIdInvalid = 0x80220000,

        /// <summary>
        /// The timestamp is outside the range allowed by the server.
        /// </summary>
        BadInvalidTimestamp = 0x80230000,

        /// <summary>
        /// The nonce does appear to be not a random value or it is not the correct length.
        /// </summary>
        BadNonceInvalid = 0x80240000,

        /// <summary>
        /// The session id is not valid.
        /// </summary>
        BadSessionIdInvalid = 0x80250000,

        /// <summary>
        /// The session was closed by the client.
        /// </summary>
        BadSessionClosed = 0x80260000,

        /// <summary>
        /// The session cannot be used because ActivateSession has not been called.
        /// </summary>
        BadSessionNotActivated = 0x80270000,

        /// <summary>
        /// The subscription id is not valid.
        /// </summary>
        BadSubscriptionIdInvalid = 0x80280000,

        /// <summary>
        /// The header for the request is missing or invalid.
        /// </summary>
        BadRequestHeaderInvalid = 0x802A0000,

        /// <summary>
        /// The timestamps to return parameter is invalid.
        /// </summary>
        BadTimestampsToReturnInvalid = 0x802B0000,

        /// <summary>
        /// The request was cancelled by the client.
        /// </summary>
        BadRequestCancelledByClient = 0x802C0000,

        /// <summary>
        /// Too many arguments were provided.
        /// </summary>
        BadTooManyArguments = 0x80E50000,

        /// <summary>
        /// The server requires a license to operate in general or to perform a service or operation, but existing license is expired.
        /// </summary>
        BadLicenseExpired = 0x810E0000,

        /// <summary>
        /// The server has limits on number of allowed operations / objects, based on installed licenses, and these limits where exceeded.
        /// </summary>
        BadLicenseLimitsExceeded = 0x810F0000,

        /// <summary>
        /// The server does not have a license which is required to operate in general or to perform a service or operation.
        /// </summary>
        BadLicenseNotAvailable = 0x81100000,

        /// <summary>
        /// The subscription was transferred to another session.
        /// </summary>
        GoodSubscriptionTransferred = 0x002D0000,

        /// <summary>
        /// The processing will complete asynchronously.
        /// </summary>
        GoodCompletesAsynchronously = 0x002E0000,

        /// <summary>
        /// Sampling has slowed down due to resource limitations.
        /// </summary>
        GoodOverload = 0x002F0000,

        /// <summary>
        /// The value written was accepted but was clamped.
        /// </summary>
        GoodClamped = 0x00300000,

        /// <summary>
        /// Communication with the data source is defined, but not established, and there is no last known value available.
        /// </summary>
        BadNoCommunication = 0x80310000,

        /// <summary>
        /// Waiting for the server to obtain values from the underlying data source.
        /// </summary>
        BadWaitingForInitialData = 0x80320000,

        /// <summary>
        /// The syntax of the node id is not valid.
        /// </summary>
        BadNodeIdInvalid = 0x80330000,

        /// <summary>
        /// The node id refers to a node that does not exist in the server address space.
        /// </summary>
        BadNodeIdUnknown = 0x80340000,

        /// <summary>
        /// The attribute is not supported for the specified Node.
        /// </summary>
        BadAttributeIdInvalid = 0x80350000,

        /// <summary>
        /// The syntax of the index range parameter is invalid.
        /// </summary>
        BadIndexRangeInvalid = 0x80360000,

        /// <summary>
        /// No data exists within the range of indexes specified.
        /// </summary>
        BadIndexRangeNoData = 0x80370000,

        /// <summary>
        /// The data encoding is invalid.
        /// </summary>
        BadDataEncodingInvalid = 0x80380000,

        /// <summary>
        /// The server does not support the requested data encoding for the node.
        /// </summary>
        BadDataEncodingUnsupported = 0x80390000,

        /// <summary>
        /// The access level does not allow reading or subscribing to the Node.
        /// </summary>
        BadNotReadable = 0x803A0000,

        /// <summary>
        /// The access level does not allow writing to the Node.
        /// </summary>
        BadNotWritable = 0x803B0000,

        /// <summary>
        /// The value was out of range.
        /// </summary>
        BadOutOfRange = 0x803C0000,

        /// <summary>
        /// The requested operation is not supported.
        /// </summary>
        BadNotSupported = 0x803D0000,

        /// <summary>
        /// A requested item was not found or a search operation ended without success.
        /// </summary>
        BadNotFound = 0x803E0000,

        /// <summary>
        /// The object cannot be used because it has been deleted.
        /// </summary>
        BadObjectDeleted = 0x803F0000,

        /// <summary>
        /// Requested operation is not implemented.
        /// </summary>
        BadNotImplemented = 0x80400000,

        /// <summary>
        /// The monitoring mode is invalid.
        /// </summary>
        BadMonitoringModeInvalid = 0x80410000,

        /// <summary>
        /// The monitoring item id does not refer to a valid monitored item.
        /// </summary>
        BadMonitoredItemIdInvalid = 0x80420000,

        /// <summary>
        /// The monitored item filter parameter is not valid.
        /// </summary>
        BadMonitoredItemFilterInvalid = 0x80430000,

        /// <summary>
        /// The server does not support the requested monitored item filter.
        /// </summary>
        BadMonitoredItemFilterUnsupported = 0x80440000,

        /// <summary>
        /// A monitoring filter cannot be used in combination with the attribute specified.
        /// </summary>
        BadFilterNotAllowed = 0x80450000,

        /// <summary>
        /// A mandatory structured parameter was missing or null.
        /// </summary>
        BadStructureMissing = 0x80460000,

        /// <summary>
        /// The event filter is not valid.
        /// </summary>
        BadEventFilterInvalid = 0x80470000,

        /// <summary>
        /// The content filter is not valid.
        /// </summary>
        BadContentFilterInvalid = 0x80480000,

        /// <summary>
        /// An unregognized operator was provided in a filter.
        /// </summary>
        BadFilterOperatorInvalid = 0x80C10000,

        /// <summary>
        /// A valid operator was provided, but the server does not provide support for this filter operator.
        /// </summary>
        BadFilterOperatorUnsupported = 0x80C20000,

        /// <summary>
        /// The number of operands provided for the filter operator was less then expected for the operand provided.
        /// </summary>
        BadFilterOperandCountMismatch = 0x80C30000,

        /// <summary>
        /// The operand used in a content filter is not valid.
        /// </summary>
        BadFilterOperandInvalid = 0x80490000,

        /// <summary>
        /// The referenced element is not a valid element in the content filter.
        /// </summary>
        BadFilterElementInvalid = 0x80C40000,

        /// <summary>
        /// The referenced literal is not a valid value.
        /// </summary>
        BadFilterLiteralInvalid = 0x80C50000,

        /// <summary>
        /// The continuation point provide is longer valid.
        /// </summary>
        BadContinuationPointInvalid = 0x804A0000,

        /// <summary>
        /// The operation could not be processed because all continuation points have been allocated.
        /// </summary>
        BadNoContinuationPoints = 0x804B0000,

        /// <summary>
        /// The operation could not be processed because all continuation points have been allocated.
        /// </summary>
        BadReferenceTypeIdInvalid = 0x804C0000,

        /// <summary>
        /// The browse direction is not valid.
        /// </summary>
        BadBrowseDirectionInvalid = 0x804D0000,

        /// <summary>
        /// The node is not part of the view.
        /// </summary>
        BadNodeNotInView = 0x804E0000,

        /// <summary>
        /// The number was not accepted because of a numeric overflow.
        /// </summary>
        BadNumericOverflow = 0x81120000,

        /// <summary>
        /// The ServerUri is not a valid URI.
        /// </summary>
        BadServerUriInvalid = 0x804F0000,

        /// <summary>
        /// No ServerName was specified.
        /// </summary>
        BadServerNameMissing = 0x80500000,

        /// <summary>
        /// No DiscoveryUrl was specified.
        /// </summary>
        BadDiscoveryUrlMissing = 0x80510000,

        /// <summary>
        /// The semaphore file specified by the client is not valid.
        /// </summary>
        BadSempahoreFileMissing = 0x80520000,

        /// <summary>
        /// The security token request type is not valid.
        /// </summary>
        BadRequestTypeInvalid = 0x80530000,

        /// <summary>
        /// The security mode does not meet the requirements set by the server.
        /// </summary>
        BadSecurityModeRejected = 0x80540000,

        /// <summary>
        /// The security policy does not meet the requirements set by the server.
        /// </summary>
        BadSecurityPolicyRejected = 0x80550000,

        /// <summary>
        /// The server has reached its maximum number of sessions.
        /// </summary>
        BadTooManySessions = 0x80560000,

        /// <summary>
        /// The user token signature is missing or invalid.
        /// </summary>
        BadUserSignatureInvalid = 0x80570000,

        /// <summary>
        /// The signature generated with the client certificate is missing or invalid.
        /// </summary>
        BadApplicationSignatureInvalid = 0x80580000,

        /// <summary>
        /// The client did not provide at least one software certificate that is valid and meets the profile requirements for the server.
        /// </summary>
        BadNoValidCertificates = 0x80590000,

        /// <summary>
        /// The server does not support changing the user identity assigned to the session.
        /// </summary>
        BadIdentityChangeNotSupported = 0x80C60000,

        /// <summary>
        /// The request was cancelled by the client with the Cancel service.
        /// </summary>
        BadRequestCancelledByRequest = 0x805A0000,

        /// <summary>
        /// The parent node id does not to refer to a valid node.
        /// </summary>
        BadParentNodeIdInvalid = 0x805B0000,

        /// <summary>
        /// The reference could not be created because it violates constraints imposed by the data model.
        /// </summary>
        BadReferenceNotAllowed = 0x805C0000,

        /// <summary>
        /// The requested node id was reject because it was either invalid or server does not allow node ids to be specified by the client.
        /// </summary>
        BadNodeIdRejected = 0x805D0000,

        /// <summary>
        /// The requested node id is already used by another node.
        /// </summary>
        BadNodeIdExists = 0x805E0000,

        /// <summary>
        /// The node class is not valid.
        /// </summary>
        BadNodeClassInvalid = 0x805F0000,

        /// <summary>
        /// The browse name is invalid.
        /// </summary>
        BadBrowseNameInvalid = 0x80600000,

        /// <summary>
        /// The browse name is not unique among nodes that share the same relationship with the parent.
        /// </summary>
        BadBrowseNameDuplicated = 0x80610000,

        /// <summary>
        /// The node attributes are not valid for the node class.
        /// </summary>
        BadNodeAttributesInvalid = 0x80620000,

        /// <summary>
        /// The type definition node id does not reference an appropriate type node.
        /// </summary>
        BadTypeDefinitionInvalid = 0x80630000,

        /// <summary>
        /// The source node id does not reference a valid node.
        /// </summary>
        BadSourceNodeIdInvalid = 0x80640000,

        /// <summary>
        /// The target node id does not reference a valid node.
        /// </summary>
        BadTargetNodeIdInvalid = 0x80650000,

        /// <summary>
        /// The reference type between the nodes is already defined.
        /// </summary>
        BadDuplicateReferenceNotAllowed = 0x80660000,

        /// <summary>
        /// The server does not allow this type of self reference on this node.
        /// </summary>
        BadInvalidSelfReference = 0x80670000,

        /// <summary>
        /// The reference type is not valid for a reference to a remote server.
        /// </summary>
        BadReferenceLocalOnly = 0x80680000,

        /// <summary>
        /// The server will not allow the node to be deleted.
        /// </summary>
        BadNoDeleteRights = 0x80690000,

        /// <summary>
        /// The server was not able to delete all target references.
        /// </summary>
        UncertainReferenceNotDeleted = 0x40BC0000,

        /// <summary>
        /// The server index is not valid.
        /// </summary>
        BadServerIndexInvalid = 0x806A0000,

        /// <summary>
        /// The view id does not refer to a valid view node.
        /// </summary>
        BadViewIdUnknown = 0x806B0000,

        /// <summary>
        /// The view timestamp is not available or not supported.
        /// </summary>
        BadViewTimestampInvalid = 0x80C90000,

        /// <summary>
        /// The view parameters are not consistent with each other.
        /// </summary>
        BadViewParameterMismatch = 0x80CA0000,

        /// <summary>
        /// The view version is not available or not supported.
        /// </summary>
        BadViewVersionInvalid = 0x80CB0000,

        /// <summary>
        /// The list of references may not be complete because the underlying system is not available.
        /// </summary>
        UncertainNotAllNodesAvailable = 0x40C00000,

        /// <summary>
        /// The server should have followed a reference to a node in a remote server but did not. The result set may be incomplete.
        /// </summary>
        GoodResultsMayBeIncomplete = 0x00BA0000,

        /// <summary>
        /// The provided Nodeid was not a type definition nodeid.
        /// </summary>
        BadNotTypeDefinition = 0x80C80000,

        /// <summary>
        /// One of the references to follow in the relative path references to a node in the address space in another server.
        /// </summary>
        UncertainReferenceOutOfServer = 0x406C0000,

        /// <summary>
        /// The requested operation has too many matches to return.
        /// </summary>
        BadTooManyMatches = 0x806D0000,

        /// <summary>
        /// The requested operation requires too many resources in the server.
        /// </summary>
        BadQueryTooComplex = 0x806E0000,

        /// <summary>
        /// The requested operation has no match to return.
        /// </summary>
        BadNoMatch = 0x806F0000,

        /// <summary>
        /// The max age parameter is invalid.
        /// </summary>
        BadMaxAgeInvalid = 0x80700000,

        /// <summary>
        /// The operation is not permitted over the current secure channel.
        /// </summary>
        BadSecurityModeInsufficient = 0x80E60000,

        /// <summary>
        /// The history details parameter is not valid.
        /// </summary>
        BadHistoryOperationInvalid = 0x80710000,

        /// <summary>
        /// The server does not support the requested operation.
        /// </summary>
        BadHistoryOperationUnsupported = 0x80720000,

        /// <summary>
        /// The defined timestamp to return was invalid.
        /// </summary>
        BadInvalidTimestampArgument = 0x80BD0000,

        /// <summary>
        /// The server does not support writing the combination of value, status and timestamps provided.
        /// </summary>
        BadWriteNotSupported = 0x80730000,

        /// <summary>
        /// The value supplied for the attribute is not of the same type as the attribute's value.
        /// </summary>
        BadTypeMismatch = 0x80740000,

        /// <summary>
        /// The method id does not refer to a method for the specified object.
        /// </summary>
        BadMethodInvalid = 0x80750000,

        /// <summary>
        /// The client did not specify all of the input arguments for the method.
        /// </summary>
        BadArgumentsMissing = 0x80760000,

        /// <summary>
        /// The executable attribute does not allow the execution of the method.
        /// </summary>
        BadNotExecutable = 0x81110000,

        /// <summary>
        /// The server has reached its  maximum number of subscriptions.
        /// </summary>
        BadTooManySubscriptions = 0x80770000,

        /// <summary>
        /// The server has reached the maximum number of queued publish requests.
        /// </summary>
        BadTooManyPublishRequests = 0x80780000,

        /// <summary>
        /// There is no subscription available for this session.
        /// </summary>
        BadNoSubscription = 0x80790000,

        /// <summary>
        /// The sequence number is unknown to the server.
        /// </summary>
        BadSequenceNumberUnknown = 0x807A0000,

        /// <summary>
        /// The requested notification message is no longer available.
        /// </summary>
        BadMessageNotAvailable = 0x807B0000,

        /// <summary>
        /// The client of the current session does not support one or more Profiles that are necessary for the subscription.
        /// </summary>
        BadInsufficientClientProfile = 0x807C0000,

        /// <summary>
        /// The sub-state machine is not currently active.
        /// </summary>
        BadStateNotActive = 0x80BF0000,

        /// <summary>
        /// An equivalent rule already exists.
        /// </summary>
        BadAlreadyExists = 0x81150000,

        /// <summary>
        /// The server cannot process the request because it is too busy.
        /// </summary>
        BadTcpServerTooBusy = 0x807D0000,

        /// <summary>
        /// The type of the message specified in the header invalid.
        /// </summary>
        BadTcpMessageTypeInvalid = 0x807E0000,

        /// <summary>
        /// The SecureChannelId and/or TokenId are not currently in use.
        /// </summary>
        BadTcpSecureChannelUnknown = 0x807F0000,

        /// <summary>
        /// The size of the message specified in the header is too large.
        /// </summary>
        BadTcpMessageTooLarge = 0x80800000,

        /// <summary>
        /// There are not enough resources to process the request.
        /// </summary>
        BadTcpNotEnoughResources = 0x80810000,

        /// <summary>
        /// An internal error occurred.
        /// </summary>
        BadTcpInternalError = 0x80820000,

        /// <summary>
        /// The server does not recognize the QueryString specified.
        /// </summary>
        BadTcpEndpointUrlInvalid = 0x80830000,

        /// <summary>
        /// The request could not be sent because of a network interruption.
        /// </summary>
        BadRequestInterrupted = 0x80840000,

        /// <summary>
        /// Timeout occurred while processing the request.
        /// </summary>
        BadRequestTimeout = 0x80850000,

        /// <summary>
        /// The secure channel has been closed.
        /// </summary>
        BadSecureChannelClosed = 0x80860000,

        /// <summary>
        /// The token has expired or is not recognized.
        /// </summary>
        BadSecureChannelTokenUnknown = 0x80870000,

        /// <summary>
        /// The sequence number is not valid.
        /// </summary>
        BadSequenceNumberInvalid = 0x80880000,

        /// <summary>
        /// The applications do not have compatible protocol versions.
        /// </summary>
        BadProtocolVersionUnsupported = 0x80BE0000,

        /// <summary>
        /// There is a problem with the configuration that affects the usefulness of the value.
        /// </summary>
        BadConfigurationError = 0x80890000,

        /// <summary>
        /// The variable should receive its value from another variable, but has never been configured to do so.
        /// </summary>
        BadNotConnected = 0x808A0000,

        /// <summary>
        /// There has been a failure in the device/data source that generates the value that has affected the value.
        /// </summary>
        BadDeviceFailure = 0x808B0000,

        /// <summary>
        /// There has been a failure in the sensor from which the value is derived by the device/data source.
        /// </summary>
        BadSensorFailure = 0x808C0000,

        /// <summary>
        /// The source of the data is not operational.
        /// </summary>
        BadOutOfService = 0x808D0000,

        /// <summary>
        /// The deadband filter is not valid.
        /// </summary>
        BadDeadbandFilterInvalid = 0x808E0000,

        /// <summary>
        /// Communication to the data source has failed. The variable value is the last value that had a good quality.
        /// </summary>
        UncertainNoCommunicationLastUsableValue = 0x408F0000,

        /// <summary>
        /// Whatever was updating this value has stopped doing so.
        /// </summary>
        UncertainLastUsableValue = 0x40900000,

        /// <summary>
        /// The value is an operational value that was manually overwritten.
        /// </summary>
        UncertainSubstituteValue = 0x40910000,

        /// <summary>
        /// The value is an initial value for a variable that normally receives its value from another variable.
        /// </summary>
        UncertainInitialValue = 0x40920000,

        /// <summary>
        /// The value is at one of the sensor limits.
        /// </summary>
        UncertainSensorNotAccurate = 0x40930000,

        /// <summary>
        /// The value is outside of the range of values defined for this parameter.
        /// </summary>
        UncertainEngineeringUnitsExceeded = 0x40940000,

        /// <summary>
        /// The value is derived from multiple sources and has less than the required number of Good sources.
        /// </summary>
        UncertainSubNormal = 0x40950000,

        /// <summary>
        /// The value has been overridden.
        /// </summary>
        GoodLocalOverride = 0x00960000,

        /// <summary>
        /// This Condition refresh failed, a Condition refresh operation is already in progress.
        /// </summary>
        BadRefreshInProgress = 0x80970000,

        /// <summary>
        /// This condition has already been disabled.
        /// </summary>
        BadConditionAlreadyDisabled = 0x80980000,

        /// <summary>
        /// This condition has already been enabled.
        /// </summary>
        BadConditionAlreadyEnabled = 0x80CC0000,

        /// <summary>
        /// Property not available, this condition is disabled.
        /// </summary>
        BadConditionDisabled = 0x80990000,

        /// <summary>
        /// The specified event id is not recognized.
        /// </summary>
        BadEventIdUnknown = 0x809A0000,

        /// <summary>
        /// The event cannot be acknowledged.
        /// </summary>
        BadEventNotAcknowledgeable = 0x80BB0000,

        /// <summary>
        /// The dialog condition is not active.
        /// </summary>
        BadDialogNotActive = 0x80CD0000,

        /// <summary>
        /// The response is not valid for the dialog.
        /// </summary>
        BadDialogResponseInvalid = 0x80CE0000,

        /// <summary>
        /// The condition branch has already been acknowledged.
        /// </summary>
        BadConditionBranchAlreadyAcked = 0x80CF0000,

        /// <summary>
        /// The condition branch has already been confirmed.
        /// </summary>
        BadConditionBranchAlreadyConfirmed = 0x80D00000,

        /// <summary>
        /// The condition has already been shelved.
        /// </summary>
        BadConditionAlreadyShelved = 0x80D10000,

        /// <summary>
        /// The condition is not currently shelved.
        /// </summary>
        BadConditionNotShelved = 0x80D20000,

        /// <summary>
        /// The shelving time not within an acceptable range.
        /// </summary>
        BadShelvingTimeOutOfRange = 0x80D30000,

        /// <summary>
        /// No data exists for the requested time range or event filter.
        /// </summary>
        BadNoData = 0x809B0000,

        /// <summary>
        /// No data found to provide upper or lower bound value.
        /// </summary>
        BadBoundNotFound = 0x80D70000,

        /// <summary>
        /// The server cannot retrieve a bound for the variable.
        /// </summary>
        BadBoundNotSupported = 0x80D80000,

        /// <summary>
        /// Data is missing due to collection started/stopped/lost.
        /// </summary>
        BadDataLost = 0x809D0000,

        /// <summary>
        /// Expected data is unavailable for the requested time range due to an un-mounted volume, an off-line archive or tape, or similar reason for temporary unavailability.
        /// </summary>
        BadDataUnavailable = 0x809E0000,

        /// <summary>
        /// The data or event was not successfully inserted because a matching entry exists.
        /// </summary>
        BadEntryExists = 0x809F0000,

        /// <summary>
        /// The data or event was not successfully updated because no matching entry exists.
        /// </summary>
        BadNoEntryExists = 0x80A00000,

        /// <summary>
        /// The client requested history using a timestamp format the server does not support (i.e requested ServerTimestamp when server only supports SourceTimestamp).
        /// </summary>
        BadTimestampNotSupported = 0x80A10000,

        /// <summary>
        /// The data or event was successfully inserted into the historical database.
        /// </summary>
        GoodEntryInserted = 0x00A20000,

        /// <summary>
        /// The data or event field was successfully replaced in the historical database.
        /// </summary>
        GoodEntryReplaced = 0x00A30000,

        /// <summary>
        /// The value is derived from multiple values and has less than the required number of Good values.
        /// </summary>
        UncertainDataSubNormal = 0x40A40000,

        /// <summary>
        /// No data exists for the requested time range or event filter.
        /// </summary>
        GoodNoData = 0x00A50000,

        /// <summary>
        /// The data or event field was successfully replaced in the historical database.
        /// </summary>
        GoodMoreData = 0x00A60000,

        /// <summary>
        /// The requested number of Aggregates does not match the requested number of NodeIds.
        /// </summary>
        BadAggregateListMismatch = 0x80D40000,

        /// <summary>
        /// The requested Aggregate is not support by the server.
        /// </summary>
        BadAggregateNotSupported = 0x80D50000,

        /// <summary>
        /// The aggregate value could not be derived due to invalid data inputs.
        /// </summary>
        BadAggregateInvalidInputs = 0x80D60000,

        /// <summary>
        /// The aggregate configuration is not valid for specified node.
        /// </summary>
        BadAggregateConfigurationRejected = 0x80DA0000,

        /// <summary>
        /// The request pecifies fields which are not valid for the EventType or cannot be saved by the historian.
        /// </summary>
        GoodDataIgnored = 0x00D90000,

        /// <summary>
        /// The request was rejected by the server because it did not meet the criteria set by the server.
        /// </summary>
        BadRequestNotAllowed = 0x80E40000,

        /// <summary>
        /// The request has not been processed by the server yet.
        /// </summary>
        BadRequestNotComplete = 0x81130000,

        /// <summary>
        /// The value does not come from the real source and has been edited by the server.
        /// </summary>
        GoodEdited = 0x00DC0000,

        /// <summary>
        /// There was an error in execution of these post-actions.
        /// </summary>
        GoodPostActionFailed = 0x00DD0000,

        /// <summary>
        /// The related EngineeringUnit has been changed but the Variable Value is still provided based on the previous unit.
        /// </summary>
        UncertainDominantValueChanged = 0x40DE0000,

        /// <summary>
        /// A dependent value has been changed but the change has not been applied to the device.
        /// </summary>
        GoodDependentValueChanged = 0x00E00000,

        /// <summary>
        /// The related EngineeringUnit has been changed but this change has not been applied to the device. The Variable Value is still dependent on the previous unit but its status is currently Bad.
        /// </summary>
        BadDominantValueChanged = 0x80E10000,

        /// <summary>
        /// A dependent value has been changed but the change has not been applied to the device. The quality of the dominant variable is uncertain.
        /// </summary>
        UncertainDependentValueChanged = 0x40E20000,

        /// <summary>
        /// A dependent value has been changed but the change has not been applied to the device. The quality of the dominant variable is Bad.
        /// </summary>
        BadDependentValueChanged = 0x80E30000,

        /// <summary>
        /// The communication layer has raised an event.
        /// </summary>
        GoodCommunicationEvent = 0x00A70000,

        /// <summary>
        /// The system is shutting down.
        /// </summary>
        GoodShutdownEvent = 0x00A80000,

        /// <summary>
        /// The operation is not finished and needs to be called again.
        /// </summary>
        GoodCallAgain = 0x00A90000,

        /// <summary>
        /// A non-critical timeout occurred.
        /// </summary>
        GoodNonCriticalTimeout = 0x00AA0000,

        /// <summary>
        /// One or more arguments are invalid.
        /// </summary>
        BadInvalidArgument = 0x80AB0000,

        /// <summary>
        /// Could not establish a network connection to remote server.
        /// </summary>
        BadConnectionRejected = 0x80AC0000,

        /// <summary>
        /// The server has disconnected from the client.
        /// </summary>
        BadDisconnect = 0x80AD0000,

        /// <summary>
        /// The network connection has been closed.
        /// </summary>
        BadConnectionClosed = 0x80AE0000,

        /// <summary>
        /// The operation cannot be completed because the object is closed, uninitialized or in some other invalid state.
        /// </summary>
        BadInvalidState = 0x80AF0000,

        /// <summary>
        /// Cannot move beyond end of the stream.
        /// </summary>
        BadEndOfStream = 0x80B00000,

        /// <summary>
        /// No data is currently available for reading from a non-blocking stream.
        /// </summary>
        BadNoDataAvailable = 0x80B10000,

        /// <summary>
        /// The asynchronous operation is waiting for a response.
        /// </summary>
        BadWaitingForResponse = 0x80B20000,

        /// <summary>
        /// The asynchronous operation was abandoned by the caller.
        /// </summary>
        BadOperationAbandoned = 0x80B30000,

        /// <summary>
        /// The stream did not return all data requested (possibly because it is a non-blocking stream).
        /// </summary>
        BadExpectedStreamToBlock = 0x80B40000,

        /// <summary>
        /// Non blocking behaviour is required and the operation would block.
        /// </summary>
        BadWouldBlock = 0x80B50000,

        /// <summary>
        /// A value had an invalid syntax.
        /// </summary>
        BadSyntaxError = 0x80B60000,

        /// <summary>
        /// The operation could not be finished because all available connections are in use.
        /// </summary>
        BadMaxConnectionsReached = 0x80B70000,
    }
}
