import {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import { Dialog, DialogPanel, DialogTitle, Transition, TransitionChild } from '@headlessui/react';
import {useNavigate} from "react-router-dom";
import {TextField} from "@mui/material";

const SuggestModal = () => {
    const {showSuggestModal, setShowSuggestModal} = useContext(GlobalContext);

    const [email, setEmail] = useState('');
    const Redirect = useNavigate();

    const handleSuggestSubmit = async () => {
        Redirect(`/u/meetings/${encodeURIComponent(email)}`);
        setShowSuggestModal(false);
    };

    return (
        <Transition show={showSuggestModal}>
            <Dialog className="relative z-30" onClose={setShowSuggestModal}>
                <TransitionChild
                    enter="ease-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in duration-200"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                >
                    <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                </TransitionChild>

                <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                    <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                        <TransitionChild
                            enter="ease-out duration-300"
                            enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                            enterTo="opacity-100 translate-y-0 sm:scale-100"
                            leave="ease-in duration-200"
                            leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                            leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                        >
                            <DialogPanel className="transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                                <div className="bg-white px-4 pb-4 pt-5 flex flex-col">
                                    <DialogTitle
                                        className="text-xl font-semibold leading-6 text-gray-900"
                                    >
                                        Enter user's email to get access of their meetings schedule
                                    </DialogTitle>
                                    <div className="mt-2">
                                        <TextField
                                            label="Email"
                                            name="Email"
                                            value={email}
                                            onChange={(e) => setEmail(e.target.value)}
                                            required
                                        />
                                    </div>
                                </div>
                                <div className="flex justify-content-end bg-gray-50 px-4 py-3 gap-y-2.5 gap-x-3">
                                    <button
                                        type="button"
                                        className="hover:bg-white hover:border-2 hover:border-black\60 bg-orange-400 px-6 py-2 rounded text-black hover:text-white
                                            hover:scale-105 transition ease-out duration-200 transform"
                                        onClick={handleSuggestSubmit}
                                    >
                                        Next
                                    </button>
                                    <button
                                        type="button"
                                        className="hover:bg-black/60 bg-white-400 border-2 border-black\60 px-6 py-2 rounded text-black hover:text-white
                                            hover:scale-105 transition ease-out duration-200 transform"
                                        onClick={() => setShowSuggestModal(false)}
                                        data-autofocus
                                    >
                                        Cancel
                                    </button>
                                </div>
                            </DialogPanel>
                        </TransitionChild>
                    </div>
                </div>
            </Dialog>
        </Transition>
    );
};

export default SuggestModal;
