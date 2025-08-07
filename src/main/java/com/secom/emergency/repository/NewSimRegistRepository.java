package com.secom.emergency.repository;

import com.secom.emergency.entity.NewSimRegist;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface NewSimRegistRepository extends JpaRepository<NewSimRegist, Long> {
    
    /**
     * 電話番号で検索
     */
    Optional<NewSimRegist> findByPhoneNumber(String phoneNumber);
    
    /**
     * ICカード番号で検索
     */
    Optional<NewSimRegist> findByIcCardNumber(String icCardNumber);
    
    /**
     * 電話番号の存在確認
     */
    boolean existsByPhoneNumber(String phoneNumber);
    
    /**
     * ICカード番号の存在確認
     */
    boolean existsByIcCardNumber(String icCardNumber);
}