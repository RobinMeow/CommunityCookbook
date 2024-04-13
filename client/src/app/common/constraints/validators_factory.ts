import { ValidatorFn, Validators } from '@angular/forms'
import { ValidationField } from './validation_field'
import { Validation } from './validation'
import { assert } from '@common/assertions'

export class ValidatorsFactory {
  static create(validationField: ValidationField): ValidatorFn[] {
    const validators: ValidatorFn[] = []

    assert(validationField, `Name did not exist in vlidationFields.`)
    for (let i = 0; i < validationField.constraints.length; i++) {
      const constraint = validationField.constraints[i]
      switch (constraint.validation) {
        case Validation.Required: {
          // value is ignored by design for required.
          validators.push(Validators.required)
          break
        }
        case Validation.Min: {
          if (validationField.dataType === 'string') {
            const min = Number(constraint.value)
            validators.push(Validators.minLength(min))
          } else {
            throw new Error(
              `Validatio.Min for DataType '${validationField.dataType}' is not supported.`
            )
          }
          break
        }
        case Validation.Max: {
          if (validationField.dataType === 'string') {
            const max = Number(constraint.value)
            validators.push(Validators.maxLength(max))
          } else {
            throw new Error(
              `Validatio.Max for DataType '${validationField.dataType}' is not supported.`
            )
          }
          break
        }

        default:
          throw new Error(
            `Validation '${constraint.validation}' not supported.`
          )
      }
    }

    return validators
  }
}
