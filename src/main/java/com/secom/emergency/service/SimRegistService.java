package com.secom.emergency.service;

import com.secom.emergency.entity.NewSimRegist;
import com.secom.emergency.repository.NewSimRegistRepository;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVRecord;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;

@Service
@Transactional
public class SimRegistService {
    
    @Autowired
    private NewSimRegistRepository simRepository;
    
    /**
     * CSVファイルからSIMデータを登録
     */
    public List<String> registerSimsFromCsv(MultipartFile file) throws IOException {
        List<String> errorMessages = new ArrayList<>();
        
        // ファイル形式チェック
        if (!isValidCsvFile(file)) {
            errorMessages.add("CSVファイル形式ではありません。");
            return errorMessages;
        }
        
        try (CSVParser parser = new CSVParser(new InputStreamReader(file.getInputStream(), StandardCharsets.UTF_8),
                CSVFormat.DEFAULT.withFirstRecordAsHeader())) {
            
            int lineNumber = 1; // ヘッダー行を考慮
            for (CSVRecord record : parser) {
                lineNumber++;
                
                try {
                    String phoneNumber = record.get(0);
                    String subscriberCode = record.get(1);
                    String icCardNumber = record.get(2);
                    String networkPin = record.get(3);
                    String simUnlockCode = record.get(4);
                    
                    // パラメータチェック
                    List<String> validationErrors = validateSimData(phoneNumber, subscriberCode, icCardNumber, 
                                                                  networkPin, simUnlockCode, lineNumber);
                    if (!validationErrors.isEmpty()) {
                        errorMessages.addAll(validationErrors);
                        continue;
                    }
                    
                    // 重複チェック
                    if (simRepository.existsByPhoneNumber(phoneNumber)) {
                        errorMessages.add(String.format("行 %d: 電話番号 %s は既に登録されています。", lineNumber, phoneNumber));
                        continue;
                    }
                    
                    if (simRepository.existsByIcCardNumber(icCardNumber)) {
                        errorMessages.add(String.format("行 %d: ICカード番号 %s は既に登録されています。", lineNumber, icCardNumber));
                        continue;
                    }
                    
                    // データベース登録
                    NewSimRegist sim = new NewSimRegist(phoneNumber, subscriberCode, icCardNumber, 
                                                       networkPin, simUnlockCode);
                    simRepository.save(sim);
                    
                } catch (Exception e) {
                    errorMessages.add(String.format("行 %d: データ処理エラー - %s", lineNumber, e.getMessage()));
                }
            }
        }
        
        return errorMessages;
    }
    
    /**
     * SIMデータのバリデーション
     */
    private List<String> validateSimData(String phoneNumber, String subscriberCode, String icCardNumber,
                                        String networkPin, String simUnlockCode, int lineNumber) {
        List<String> errors = new ArrayList<>();
        
        // 電話番号チェック（11桁の数字）
        if (phoneNumber == null || !phoneNumber.matches("\\d{11}")) {
            errors.add(String.format("行 %d: 電話番号は11桁の数字で入力してください。", lineNumber));
        }
        
        // 加入者コードチェック（8桁の英数字）
        if (subscriberCode == null || !subscriberCode.matches("[A-Za-z0-9]{8}")) {
            errors.add(String.format("行 %d: 加入者コードは8桁の英数字で入力してください。", lineNumber));
        }
        
        // ICカード番号チェック（19桁の数字）
        if (icCardNumber == null || !icCardNumber.matches("\\d{19}")) {
            errors.add(String.format("行 %d: ICカード番号は19桁の数字で入力してください。", lineNumber));
        }
        
        // ネットワーク暗証番号チェック（4桁の数字）
        if (networkPin == null || !networkPin.matches("\\d{4}")) {
            errors.add(String.format("行 %d: ネットワーク暗証番号は4桁の数字で入力してください。", lineNumber));
        }
        
        // SIMロック解除コードチェック（8桁の英数字）
        if (simUnlockCode == null || !simUnlockCode.matches("[A-Za-z0-9]{8}")) {
            errors.add(String.format("行 %d: SIMロック解除コードは8桁の英数字で入力してください。", lineNumber));
        }
        
        return errors;
    }
    
    /**
     * CSVファイル形式チェック
     */
    private boolean isValidCsvFile(MultipartFile file) {
        if (file == null || file.isEmpty()) {
            return false;
        }
        
        String filename = file.getOriginalFilename();
        return filename != null && filename.toLowerCase().endsWith(".csv");
    }
    
    /**
     * 全てのSIMデータを取得
     */
    @Transactional(readOnly = true)
    public List<NewSimRegist> getAllSims() {
        return simRepository.findAll();
    }
    
    /**
     * ICカード番号で検索
     */
    @Transactional(readOnly = true)
    public NewSimRegist findByIcCardNumber(String icCardNumber) {
        return simRepository.findByIcCardNumber(icCardNumber).orElse(null);
    }
}