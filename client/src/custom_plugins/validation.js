var nameRegex = /^[A-Z][a-zA-Z]*(?:\s[a-zA-Z]+)*$/;
var passwordRegex = /^[\w!@#$%^&*]*$/;

export const NAME_MESSAGE = "Use only latin letters and spaces. Do not use space as first and last character and more than one in a row. First letter must be capital.";
export const PASSWORD_MESSAGE = "Use only latin letters, numbers and special characters: !, @, #, $, %, ^, &, *";
export const EXISTS = "Already exists";
export const MUST_BE_EMPTY = "Must be empty";
export const INCORRECT_DATA = "Incorrect data. Check login and password";
export const UNAUTHORIZED = "Please log in to continue";
export const INTERNAL = "Internal server error";

export const name = (value) => nameRegex.test(value);
export const password = (value) => passwordRegex.test(value);
export const ok = () => true;