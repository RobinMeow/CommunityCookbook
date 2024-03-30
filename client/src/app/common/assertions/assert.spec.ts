import { assert } from './assert'

describe('assert should', () => {
  it('throw error for false condition', () => {
    expect(() => assert(false, 'error message')).toThrowError()
  })

  const errorMessages = ['ABC', 'sentence with spaces', '', ' ']
  errorMessages.forEach((testCase: string) => {
    it(`should contain error message '${testCase}' when thrown`, () => {
      expect(() => assert(false, testCase)).toThrowError(testCase)
    })
  })

  // TODO im considerung, throwing errors in assert() for empty strings and if the string doesnt end with an dot.

  it('not throw for true condition', () => {
    expect(() => assert(true, '')).not.toThrowError()
  })
})
