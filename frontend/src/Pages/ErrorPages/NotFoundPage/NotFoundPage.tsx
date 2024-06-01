import {useLocation} from "react-router-dom";
import '../NotFoundPage/NotFoundPage.css'

function NotFoundPage() {
    const location = useLocation();
    const errorMessage = location.state?.errorMessage || 'Page Not Found';
    return (
        <div className={"NotFoundPage-wrapper"}>
            <div className={'error-header'}>
                <h1>404 Not Found</h1>
            </div>
            <div className="error-message">
                <p>Сообщение об ошибке: {errorMessage}</p>
            </div>
            <div className={"gif-wrapper"}>
                <img src="https://gifdb.com/images/high/rick-roll-ashley-dance-h2d7puir23see4lq.gif" alt="Гифка"/>
            </div>
        </div>
    );
}

export default NotFoundPage;