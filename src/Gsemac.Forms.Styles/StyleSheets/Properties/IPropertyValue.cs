﻿using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyValue {

        Type Type { get; }
        object Value { get; }

        bool IsKeyword { get; }
        bool IsVariableReference { get; }

    }

    public interface IPropertyValue<T> :
        IPropertyValue {

        new T Value { get; }

    }

}