using System;

using R5T.L0065.T000;
using R5T.T0132;
using R5T.T0146;
using R5T.T0146.Extensions;


namespace R5T.S0103
{
    [FunctionalityMarker]
    public partial interface ISignatureEqualityOperator : IFunctionalityMarker
    {
        public Result<bool> Are_Equal(Signature signatureA, Signature signatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("Signatures-Equal.")
                ;

            var nullCheckDeterminesEquality = Instances.EqualityOperator_Results.NullCheckDeterminesEquality(
                signatureA,
                signatureB,
                out var nullCheckResult);

            result.WithChild(nullCheckResult);

            if (nullCheckDeterminesEquality)
            {
                result.WithValue(nullCheckResult.Value);

                // One or both instances are null, so we can't check the type of both instances without getting a null reference exception.
                return result;
            }

            var typeCheckDeterminesEquality = Instances.EqualityOperator_Results.TypeCheckDeterminesEquality_WithoutNullCheck(
                signatureA,
                signatureB,
                out var typesAreEqualResult);

            result.WithChild(typesAreEqualResult);

            if (typeCheckDeterminesEquality)
            {
                result.WithValue(typesAreEqualResult.Value);

                // The types are not the same, so can't test values.
                return result;
            }

            var signatureTypeResults = Instances.SignatureOperator.SignatureTypeSwitch(
                signatureA,
                signatureB,
                this.Are_Equal,
                this.Are_Equal,
                this.Are_Equal,
                this.Are_Equal,
                this.Are_Equal);

            result.WithChild(signatureTypeResults);

            result.AddChildFailuresFailure_IfChildFailures();

            return result;
        }

        public void Are_Equal_SignatureOnly(
            Signature signatureA,
            Signature signatureB,
            Result<bool> resultToModify)
        {
            var kindMarkersEqual = signatureA.KindMarker == signatureB.KindMarker;
            if(!kindMarkersEqual)
            {
                var failureMessage = $"{signatureA.KindMarker}, {signatureB.KindMarker}: Different kind markers.";

                resultToModify
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var isObsoleteEqual = signatureA.IsObsolete == signatureB.IsObsolete;
            if(!isObsoleteEqual)
            {
                var failureMessage = $"{signatureA.IsObsolete}, {signatureB.IsObsolete}: Different is-obsolete values.";

                resultToModify
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }
        }

        public Result<bool> Are_Equal(EventSignature eventSignatureA, EventSignature eventSignatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("EventSignatures-Equal")
                // Assume success.
                .WithValue(true)
                ;

            this.Are_Equal_SignatureOnly(eventSignatureA, eventSignatureB, result);

            var eventNamesEqual = eventSignatureA.EventName == eventSignatureB.EventName;
            if(!eventNamesEqual)
            {
                var failureMessage = $"{eventSignatureA.EventName}, {eventSignatureB.EventName}: Different event names.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var declaringTypeResult = this.Are_Equal(
                eventSignatureA.DeclaringType,
                eventSignatureB.DeclaringType);

            declaringTypeResult.AddChildFailuresFailure_IfChildFailures();

            if(!declaringTypeResult)
            {
                var failureMessage = $"Declaring types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(declaringTypeResult)
                    ;
            }

            var eventHandlerTypeResult = this.Are_Equal(
                eventSignatureA.EventHandlerType,
                eventSignatureB.EventHandlerType);

            eventHandlerTypeResult.AddChildFailuresFailure_IfChildFailures();

            if(!eventHandlerTypeResult)
            {
                var failureMessage = $"Event handler types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(eventHandlerTypeResult)
                    ;
            }

            return result;
        }

        public Result<bool> Are_Equal(FieldSignature fieldSignatureA, FieldSignature fieldSignatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("FieldSignatures-Equal")
                ;

            this.Are_Equal_SignatureOnly(fieldSignatureA, fieldSignatureB, result);

            var fieldNamesEqual = fieldSignatureA.FieldName == fieldSignatureB.FieldName;
            if (!fieldNamesEqual)
            {
                var failureMessage = $"{fieldSignatureA.FieldName}, {fieldSignatureB.FieldName}: Different field names.";

                result.WithFailure(failureMessage);
            }

            var declaringTypeResult = this.Are_Equal(
                fieldSignatureA.DeclaringType,
                fieldSignatureB.DeclaringType);

            declaringTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!declaringTypeResult)
            {
                var failureMessage = $"Declaring types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(declaringTypeResult)
                    ;
            }

            var fieldTypeResult = this.Are_Equal(
                fieldSignatureA.FieldType,
                fieldSignatureB.FieldType);

            fieldTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!fieldTypeResult)
            {
                var failureMessage = $"Field types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(fieldTypeResult)
                    ;
            }

            return result;
        }

        public Result<bool> Are_Equal(PropertySignature propertySignatureA, PropertySignature propertySignatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("PropertySignatures-Equal")
                ;

            this.Are_Equal_SignatureOnly(propertySignatureA, propertySignatureB, result);

            var fieldNamesEqual = propertySignatureA.PropertyName == propertySignatureB.PropertyName;
            if (!fieldNamesEqual)
            {
                var failureMessage = $"{propertySignatureA.PropertyName}, {propertySignatureB.PropertyName}: Different field names.";

                result.WithFailure(failureMessage);
            }

            var declaringTypeResult = this.Are_Equal(
                propertySignatureA.DeclaringType,
                propertySignatureB.DeclaringType);

            declaringTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!declaringTypeResult)
            {
                var failureMessage = $"Declaring types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(declaringTypeResult)
                    ;
            }

            var propertyTypeResult = this.Are_Equal(
                propertySignatureA.PropertyType,
                propertySignatureB.PropertyType);

            propertyTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!propertyTypeResult)
            {
                var failureMessage = $"Property types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(propertyTypeResult)
                    ;
            }

            var parametersResult = this.Are_Equal(
                propertySignatureA.Parameters,
                propertySignatureB.Parameters);

            parametersResult.AddChildFailuresFailure_IfChildFailures();

            if (!parametersResult)
            {
                var failureMessage = $"Parameters not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(parametersResult)
                    ;
            }

            return result;
        }

        public Result<bool> Are_Equal(MethodParameter[] methodParametersA, MethodParameter[] methodParametersB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("Method Parameter Arrays-Equal")
                ;

            var nullCheckDeterminesEquality = Instances.EqualityOperator_Results.NullCheckDeterminesEquality(
                methodParametersA,
                methodParametersB,
                out var nullCheckResult);

            result.WithChild(nullCheckResult);

            if (nullCheckDeterminesEquality)
            {
                result.WithValue(nullCheckResult.Value);

                // One or both instances are null, so we can't check the type of both instances without getting a null reference exception.
                return result;
            }

            Instances.EqualityOperator_Results.Array(
                methodParametersA,
                methodParametersB,
                this.Are_Equal,
                result);

            return result;
        }

        public Result<bool> Are_Equal(MethodParameter methodParameterA, MethodParameter methodParameterB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("Method Parameters-Equal")
                ;

            var parameterNamesEqual = methodParameterA.ParameterName == methodParameterB.ParameterName;
            if (!parameterNamesEqual)
            {
                var failureMessage = $"{methodParameterA.ParameterName}, {methodParameterB.ParameterName}: Different method parameter names.";

                result.WithFailure(failureMessage);
            }

            var parameterTypeResult = this.Are_Equal(
                methodParameterA.ParameterType,
                methodParameterB.ParameterType);

            parameterTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!parameterTypeResult)
            {
                var failureMessage = $"Parameter types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(parameterTypeResult)
                    ;
            }

            return result;
        }

        public Result<bool> Are_Equal(MethodSignature methodSignatureA, MethodSignature methodSignatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("MethodSignatures-Equal")
                // Assume success.
                .WithValue(true)
                ;

            this.Are_Equal_SignatureOnly(methodSignatureA, methodSignatureB, result);

            var methodNamesEqual = methodSignatureA.MethodName == methodSignatureB.MethodName;
            if (!methodNamesEqual)
            {
                var failureMessage = $"{methodSignatureA.MethodName}, {methodSignatureB.MethodName}: Different method names.";

                result.WithFailure(failureMessage);
            }

            var declaringTypeResult = this.Are_Equal(
                methodSignatureA.DeclaringType,
                methodSignatureB.DeclaringType);

            declaringTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!declaringTypeResult)
            {
                var failureMessage = $"Declaring types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(declaringTypeResult)
                    ;
            }

            var returnTypeResult = this.Are_Equal(
                methodSignatureA.ReturnType,
                methodSignatureB.ReturnType);

            returnTypeResult.AddChildFailuresFailure_IfChildFailures();

            if (!returnTypeResult)
            {
                var failureMessage = $"Return types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(returnTypeResult)
                    ;
            }

            var parametersResult = this.Are_Equal(
                methodSignatureA.Parameters,
                methodSignatureB.Parameters);

            parametersResult.AddChildFailuresFailure_IfChildFailures();

            if (!parametersResult)
            {
                var failureMessage = $"Parameters not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(parametersResult)
                    ;
            }

            var genericTypeInputsResult = this.Are_Equal(
                methodSignatureA.GenericTypeInputs,
                methodSignatureB.GenericTypeInputs);

            genericTypeInputsResult.AddChildFailuresFailure_IfChildFailures();

            if (!genericTypeInputsResult)
            {
                var failureMessage = $"Generic type inputs not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(genericTypeInputsResult)
                    ;
            }

            return result;
        }

        public Result<bool> Are_Equal(TypeSignature[] typeSignaturesA, TypeSignature[] typeSignaturesB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("Type Signature Arrays-Equal")
                ;

            var nullCheckDeterminesEquality = Instances.EqualityOperator_Results.NullCheckDeterminesEquality(
                typeSignaturesA,
                typeSignaturesB,
                out var nullCheckResult);

            result.WithChild(nullCheckResult);

            if (nullCheckDeterminesEquality)
            {
                result.WithValue(nullCheckResult.Value);

                // One or both instances are null, so we can't check the type of both instances without getting a null reference exception.
                return result;
            }

            Instances.EqualityOperator_Results.Array(
                typeSignaturesA,
                typeSignaturesB,
                this.Are_Equal,
                result);

            return result;
        }

        public Result<bool> Are_Equal(TypeSignature typeSignatureA, TypeSignature typeSignatureB)
        {
            var result = Instances.ResultOperator.New<bool>()
                .WithTitle("TypeSignatures-Equal")
                // Assume success.
                .WithValue(true)
                ;

            var nullCheckDeterminesEquality = Instances.EqualityOperator_Results.NullCheckDeterminesEquality(
                typeSignatureA,
                typeSignatureB,
                out var nullCheckResult);

            result.WithChild(nullCheckResult);

            if (nullCheckDeterminesEquality)
            {
                result.WithValue(nullCheckResult.Value);

                // One or both instances are null, so we can't check the type of both instances without getting a null reference exception.
                return result;
            }

            this.Are_Equal_SignatureOnly(typeSignatureA, typeSignatureB, result);

            var typeNamesEqual = typeSignatureA.TypeName == typeSignatureB.TypeName;
            if (!typeNamesEqual)
            {
                var typeNameA = Instances.TextOperator.Get_TextRepresentation(typeSignatureA.TypeName);
                var typeNameB = Instances.TextOperator.Get_TextRepresentation(typeSignatureB.TypeName);

                var failureMessage = $"{typeNameA}, {typeNameB}: Different type names.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var namespaceNamesEqual = typeSignatureA.NamespaceName == typeSignatureB.NamespaceName;
            if (!namespaceNamesEqual)
            {
                var namespaceNameA = Instances.TextOperator.Get_TextRepresentation(typeSignatureA.NamespaceName);
                var namespaceNameB = Instances.TextOperator.Get_TextRepresentation(typeSignatureB.NamespaceName);

                var failureMessage = $"{namespaceNameA}, {namespaceNameB}: Different namespace names.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var isNestedEqual = typeSignatureA.Is_Nested == typeSignatureB.Is_Nested;
            if (!isNestedEqual)
            {
                var failureMessage = $"{typeSignatureA.Is_Nested}, {typeSignatureB.Is_Nested}: Different is-nested values.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var nestedTypeParentResult = this.Are_Equal(
                typeSignatureA.NestedTypeParent,
                typeSignatureB.NestedTypeParent);

            nestedTypeParentResult.AddChildFailuresFailure_IfChildFailures();

            if(!nestedTypeParentResult)
            {
                var failureMessage = $"Nested type parents not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(nestedTypeParentResult)
                    ;
            }

            var isGenericMethodParameterEqual = typeSignatureA.Is_GenericMethodParameter == typeSignatureB.Is_GenericMethodParameter;
            if (!isGenericMethodParameterEqual)
            {
                var failureMessage = $"{typeSignatureA.Is_GenericMethodParameter}, {typeSignatureB.Is_GenericMethodParameter}: Different is-generic method parameter values.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var isGenericTypeParameterEqual = typeSignatureA.Is_GenericTypeParameter == typeSignatureB.Is_GenericTypeParameter;
            if (!isGenericTypeParameterEqual)
            {
                var failureMessage = $"{typeSignatureA.Is_GenericTypeParameter}, {typeSignatureB.Is_GenericTypeParameter}: Different is-generic type parameter values.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var genericTypeInputsResult = this.Are_Equal(
                typeSignatureA.GenericTypeInputs,
                typeSignatureB.GenericTypeInputs);

            genericTypeInputsResult.AddChildFailuresFailure_IfChildFailures();

            if(!genericTypeInputsResult)
            {
                var failureMessage = $"Generic type inputs not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(genericTypeInputsResult)
                    ;
            }

            var hasElementTypeEqual = typeSignatureA.Has_ElementType == typeSignatureB.Has_ElementType;
            if (!hasElementTypeEqual)
            {
                var failureMessage = $"{typeSignatureA.Has_ElementType}, {typeSignatureB.Has_ElementType}: Different has-element type values.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var elementTypeRelationshipsEqual = typeSignatureA.ElementTypeRelationships == typeSignatureB.ElementTypeRelationships;
            if (!elementTypeRelationshipsEqual)
            {
                var failureMessage = $"{typeSignatureA.ElementTypeRelationships}, {typeSignatureB.ElementTypeRelationships}: Different element type relationships values.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    ;
            }

            var elementTypeResult = this.Are_Equal(
                typeSignatureA.ElementType,
                typeSignatureB.ElementType);

            elementTypeResult.AddChildFailuresFailure_IfChildFailures();

            if(!elementTypeResult)
            {
                var failureMessage = $"Element types not equal.";

                result
                    .WithValue(false)
                    .WithFailure(failureMessage)
                    .WithChild(genericTypeInputsResult)
                    ;
            }

            return result;
        }
    }
}
