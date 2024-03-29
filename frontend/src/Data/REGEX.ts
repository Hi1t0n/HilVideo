/* Регулярные выражения */
export const EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+.+.[a-zA-Z]{2,4}$/i;
export const LOGIN_REGEX = /^[a-zA-Z0-9_-]{3,30}$/;
export const PASSWORD_REGEX = /^[a-zA-Z0-9@#$&*]{6,30}$/;
export const PHONENUMBER_REGEX = /^[0-9]{11}$/;