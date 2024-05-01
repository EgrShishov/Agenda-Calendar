import {useContext} from "react";
import GlobalContext from "../context/globalContext.ts";

const CreateButton = () => {
    const { setShowEventDetails } = useContext(GlobalContext)

    const onClickHandler = () => setShowEventDetails(true);

    return (
        <button
            onClick={onClickHandler}
            className="text-orange-400 font-bold border p-2 rounded-full flex items-center shadow-md hover:shadow-2xl bg-white">
            <span className="material-icons-outlined w-7 h-7">
                add
            </span>
            <span className="pl-3 pr-7"> Create</span>
        </button>
    );
};

export default CreateButton;
