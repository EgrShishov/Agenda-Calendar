import calendarImage from '../../public/agendaLogo.png'
import hourglassesGif from '../../public/hourglasses.gif'


const HomeComponent = () => {
    return (
        <div className="w-full h-screen">
            <div className="flex flex-col justify-center items-center h-full">
                <p className="text-gray-400 text-3xl">
                    <span className="text-orange-400 font-bold text-4xl">Agenda </span>
                    - where every second counts.
                </p>
                <p className="text-gray-400 text-2xl text-center">
                    Powerful instrument that <span className="text-orange-400 text-2xl">helps</span> you
                    <span className="text-orange-400 text-2xl"> manage</span> your time.
                    <br/>
                    Don’t miss a moment, because every minute can unlock new opportunities.
                    <br/>
                    <span className="text-orange-400 font-bold text-3xl">Agenda Calendar </span>
                    - your <span className="text-orange-400 text-2xl">key</span> to time
                    <span className="text-orange-400 text-2xl"> management</span>,
                    your <span className="text-orange-400 text-2xl">tool</span> for crafting history!
                </p>
                {/*<img src={hourglassesGif} alt={"calendar image"} className="mr-2 mt-3.5 h-96"/>*/}
            </div>
        </div>
    );
}

export default HomeComponent;