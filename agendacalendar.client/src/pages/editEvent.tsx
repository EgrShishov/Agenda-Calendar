import {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {useParams} from "react-router-dom";

const EditEvent = () => {
    const {selectedEvent, setSelectedEvent } = useContext(GlobalContext);
    const [title, setTitle] = useState(selectedEvent._def ? selectedEvent._def.title : '');
    const [description, setDescription] = useState(selectedEvent._def ? selectedEvent._def.extendedProps.description : '');
    const [startTime, setStartTime] = useState(selectedEvent._def ? selectedEvent._def.start : '');
    const [endTime, setEndTime] = useState(selectedEvent._def ? selectedEvent._def.end : '');
    const [location, setLocation] = useState(selectedEvent._def ? selectedEvent._def.location : '');

    console.log(selectedEvent._def);
    const { id } = useParams();
    console.log(id);

    return (
      <div>
          <input
               type="text"
               name="description"
               placeholder="Add a description"
               value={description}
               required
               className="mx-3 pt-3 border-0 text-gray-600 pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
               onChange={(e) => setDescription(e.target.value)}
          />

      </div>
    );
};

export default EditEvent;