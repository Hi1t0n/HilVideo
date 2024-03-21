import "./TextInput.css"
interface Props {
    id: string;
    type: "email" | "tel" | "password" | "text";
    placeholder: string;
    value: string;
    required: boolean;
    minLength?: number;
    maxLength?: number;
    pattern?: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}
function TextInput({id, type, placeholder, value, required, minLength, maxLength, pattern, onChange} : Props){
    return(
        <input className={"text-input"} id={id} type={type} placeholder={required ? placeholder + '*' : placeholder} value={value} required={required} minLength={minLength} maxLength={maxLength} pattern={pattern} onChange={onChange}/>
    );
}

export default TextInput;