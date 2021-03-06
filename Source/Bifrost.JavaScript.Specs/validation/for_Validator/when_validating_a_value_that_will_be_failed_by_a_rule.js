﻿describe("when validating a value that will be failed by a rule", function () {
    var validator;
    var options = {
        someRule: {
            message: "The message"
        }
    };
    beforeEach(function () {
        Bifrost.validation.Rule = {
            create: function (ruleName, options) {
                return {
                    message: options.message,
                    validate: function (value, options) {
                        return false;
                    }
                }
            }
        }

        validator = Bifrost.validation.Validator.create(options);
    });

    it("should set isValid to false", function () {
        validator.validate("something");
        expect(validator.isValid()).toBeFalsy();
    });

    it("should set message", function () {
        validator.validate("something");
        expect(validator.message()).toBe(options.someRule.message);
    });
});