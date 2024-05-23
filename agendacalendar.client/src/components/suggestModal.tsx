import {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {useNavigate} from "react-router-dom";
import Example from "./example.tsx";

const SuggestModal = () => {
    const {showSuggestModal, setShowSuggestModal} = useContext(GlobalContext);

    const [email, setEmail] = useState('');
    const Redirect = useNavigate();

    const handleSuggestSubmit = () => {
        Redirect('/u/meetings')
        setShowSuggestModal(false);
    };

    return (
        <div>
            <Example open={showSuggestModal} setOpen={setShowSuggestModal}/>
        </div>
    )
};

export default SuggestModal;