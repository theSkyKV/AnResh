var passwordRegex = /^[\w!@]*$/;
var nameRegex = /^[a-zA-Z]+(?:\s[a-zA-Z]+)*$/;
var skillNameRegex = /^[\w\.#]+(?:\s[\w\.#]+)*$/;

export const PASSWORD_MESSAGE = "Use only latin letters, numbers, !, @";
export const NAME_MESSAGE = "Use only latin letters and spaces. Do not use space as first and last character and more than one in a row.";
export const SKILL_NAME_MESSAGE = "Use only latin letters, numbers, spaces, ., #. Do not use space as first and last character and more than one in a row.";

export const password = (value) => passwordRegex.test(value);
export const name = (value) => nameRegex.test(value);
export const skillName = (value) => skillNameRegex.test(value);