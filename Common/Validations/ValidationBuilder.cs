﻿namespace Common.Validations;

/// <summary>
/// The Builder is not only a shorthand for syntax, but it ensures correct fieldname captalization conventions.
/// </summary>
public sealed class ValidationBuilder
{
    private string? _fieldName = null;
    private uint? _min = null;
    private uint? _max = null;
    private bool _required = false;

    private readonly Dictionary<string, FieldConstraints> _buildedDictionary = new Dictionary<string, FieldConstraints>();

    private void AddCurrentToBuild()
    {
        if (_fieldName == null) return;

        _buildedDictionary.Add(
            new FieldName(_fieldName).ToString(),
            new FieldConstraints()
            {
                IsRequired = _required,
                Max = _max,
                Min = _min
            });
    }

    public ValidationBuilder AddField(string name)
    {
        AddCurrentToBuild();
        _fieldName = name;
        return this;
    }

    public ValidationBuilder Required()
    {
        AssertFieldIsSet();
        if (_required)
            throw new InvalidOperationException($"{nameof(Required)} Constraint has been set already.");

        _required = true;
        return this;
    }

    public ValidationBuilder Min(uint min)
    {
        AssertFieldIsSet();
        if (_min != null)
            throw new InvalidOperationException($"{nameof(Min)} Constraint has been set already.");

        _min = min;
        return this;
    }

    public ValidationBuilder Max(uint max)
    {
        AssertFieldIsSet();
        if (_max != null)
            throw new InvalidOperationException($"{nameof(Max)} Constraint has been set already.");

        _max = max;
        return this;
    }

    private void AssertFieldIsSet()
    {
        if (_fieldName == null)
            throw new InvalidOperationException($"{nameof(AddField)} has to be set before adding the constraints.");
    }

    public Dictionary<string, FieldConstraints> Build()
    {
        AddCurrentToBuild();
        return _buildedDictionary;
    }
}
