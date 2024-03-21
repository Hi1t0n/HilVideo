import "./Footer.css"
import ImageButton from "../ImageButton/ImageButton";
import TelegramLogo from "../../image/telegram_logo.svg"
import VkLogo from "../../image/vk_logo.svg"
function Footer() {
    const telegramUrl : string =  "https://t.me/Artyom_Hilton"
    const vkUrl : string = "https://vk.com/da_da_ya_hilton"

    return (
        <footer className={"footer"}>
            <div>
                <ImageButton src={TelegramLogo} url={telegramUrl} />
                <ImageButton src={VkLogo} url={vkUrl}/>
            </div>
        </footer>
    )
}

export default Footer;
